using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Archimedes.Framework.Context;
using Archimedes.Framework.ContextEnvironment;
using Archimedes.Framework.DI.Attribute;
using Archimedes.Framework.DI.Config;
using Archimedes.Framework.DI.Factories;
using Archimedes.Framework.Stereotype;
using Archimedes.Framework.Util;
using log4net;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// A very lightweight dependency injection container wich requires virtually no configuration.
    /// Use <see cref="ApplicationContext"/> with its Auto-Scan ability to get started easily.
    /// </summary>
    public class ElderBox
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<Type, object> _serviceRegistry = new Dictionary<Type, object>();
        private readonly IModuleConfiguration _configuration;
        private readonly string _name;

        #endregion

        #region Constructor

        public ElderBox()
            : this(new ElderModuleConfiguration())
        {
        }

        public ElderBox(IModuleConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            _configuration = configuration;
            UpdateSingletonInstance(typeof(ElderBox), this); // Register the current DI container context
        }

        #endregion

        #region Properties

        /// <summary>
        /// Exposes the container configuration
        /// </summary>
        public IModuleConfiguration Configuration
        {
            get { return _configuration;}
        }

        /// <summary>
        /// The name of the context of this container
        /// </summary>
        public string ContextName
        {
            get { return _name; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new instance of the given Type.
        /// Using the provided (optional) arguments as constructor parameters.
        /// If there are Constructor parameters which are not provided, this context is used to auto-wire 
        /// the missing dependencies.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Create<T>(params object[] args)
        {
            return (T)Create(typeof(T), args);
        }

        /// <summary>
        /// Creates a new instance of the given Type.
        /// Using the provided (optional) arguments as constructor parameters.
        /// If there are Constructor parameters which are not provided, this context is used to auto-wire 
        /// the missing dependencies.
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public object Create(Type t, params object[] args)
        {
            var factory = new TypeComponentFactory(t, null, false, null);
            return CreateInstance(factory, new HashSet<Type>(), args);
        }


        /// <summary>
        /// Resolve an instance for the given Type. All dependencies will be auto wired (injected).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve an instance for the given Type. All dependencies will be auto wired (injected).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public object Resolve(Type type)
        {
            return Resolve(type, new HashSet<Type>());
        }

        /// <summary>
        /// Auto wire fields / property dependencies which are annotated with <see cref="InjectAttribute"/> 
        /// Usefull if you have already an instance which could not be created using this DI container.
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="AutowireException">Thrown when autowiring of the given instance failed.</exception>
        [DebuggerStepThrough]
        public void Autowire(object instance)
        {
            Autowire(instance, new HashSet<Type>());
        }



        #endregion

        #region Internal methods

        [Obsolete("Use the module configuration to register singleton instances!")]
        internal void RegisterInstance<T>(T serviceInstance)
        {
            UpdateSingletonInstance(typeof(T), serviceInstance);
        }

        /// <summary>
        /// Autowires all fields which have the [Inject] Attribute with the resolved dependency.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <exception cref="AutowireException">Thrown when autowiring of the given instance failed.</exception>
        [DebuggerStepThrough]
        internal void Autowire(object instance, ISet<Type> unresolvedDependencies)
        {
            try
            {
                var targetFields = from f in instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                   where f.IsDefined(typeof(InjectAttribute), false)
                                   select f;

                foreach (var targetField in targetFields)
                {
                    if (targetField.GetValue(instance) == null) // Only autowire if the field is null
                    {

                        if (unresolvedDependencies.Contains(targetField.FieldType))
                        {
                            throw new CircularDependencyException(instance.GetType(), targetField.FieldType);
                        }

                        try
                        {
                            var fieldValue = Resolve(targetField.FieldType, unresolvedDependencies);
                            targetField.SetValue(instance, fieldValue);
                        }
                        catch (Exception e)
                        {
                            if (e is CircularDependencyException)
                            {
                                throw;
                            }
                            throw new AutowireException("Autowiring of Field " + targetField.Name + "(" + targetField.FieldType.Name + ") has failed!", e);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e is CircularDependencyException)
                {
                    throw;
                }
                throw new AutowireException("Autowiring of instance " + instance.GetType().Name + " failed!", e);
            }

            AutowireConfiguration(instance, unresolvedDependencies);
        }

        /// <summary>
        /// Resolves all parameter instances of the given constructor.
        /// </summary>
        /// <param name="contextInfo"></param>
        /// <param name="parameterInfos"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        internal object[] AutowireParameters(object contextInfo, ParameterInfo[] parameterInfos, ISet<Type> unresolvedDependencies, object[] providedParameters)
        {
            if (parameterInfos == null) throw new ArgumentNullException("parameterInfos");

            var parameters = new List<object>();

            foreach (var parameter in parameterInfos)
            {
                object paramInstance = null;

                if (providedParameters != null)
                {
                    paramInstance = FindParamInstance(parameter, providedParameters);
                }

                if (paramInstance == null)
                {
                    paramInstance = ResolveParameterInstance(contextInfo, parameter, unresolvedDependencies);
                }
                parameters.Add(paramInstance);
            }
            return parameters.ToArray();
        }

        #endregion

        #region Private methods

        [DebuggerStepThrough]
        private object Resolve(Type type, ISet<Type> unresolvedDependencies)
        {
            if (type == null) throw new ArgumentNullException("type");


            if (_serviceRegistry.ContainsKey(type))
            {
                return _serviceRegistry[type];
            }

            unresolvedDependencies.Add(type); // Mark this type as unresolved
            var instance = ResolveInstanceFor(type, unresolvedDependencies);

            if (instance == null)
            {
                throw new ComponentCreationException("Something went wrong while resolving instance for type " + type.Name);
            }

            return instance;
        }

        


        /// <summary>
        /// Autofills all fields of the given instance which have the [Value(...)] atribute.
        /// </summary>
        /// <param name="instance"></param>
        private void AutowireConfiguration(object instance, ISet<Type> unresolvedDependencies)
        {
            var valueFields = (from f in instance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                               where f.IsDefined(typeof(ValueAttribute), false)
                               select f).ToList();

            if (valueFields.Any())
            {
                var configurationService = (IEnvironmentService)Resolve(typeof(IEnvironmentService), unresolvedDependencies);

                var configurator = new ValueConfigurator(configurationService.Configuration);

                foreach (var targetField in valueFields)
                {
                    if (targetField.FieldType.IsValueType ||
                        targetField.GetValue(instance) == null) // Only inject value if field is null
                    {
                        var valueAttrs = targetField.GetCustomAttributes(typeof (ValueAttribute), false);
                        if (valueAttrs.Length > 0)
                        {
                            var valueAttr = (ValueAttribute) valueAttrs[0];
                            configurator.SetValueSave(targetField, instance, valueAttr.Expression);
                        }
                    }
                }
            }
        }



        private object ResolveInstanceFor(Type type, ISet<Type> unresolvedDependencies)
        {
            if(type == null) throw new ArgumentNullException("type");

            object instance = null;

            // Maybe this type has already been created
            if (_serviceRegistry.ContainsKey(type))
            {
                return _serviceRegistry[type];
            }

            // First check if we have a mapping for the given type
            var factory = _configuration.GetFactoryForType(type);


            if (factory != null)
            {
                instance = CreateInstance(factory, unresolvedDependencies);

                var implementationTargetInterfaces = FetchImplementationTargets(instance.GetType());

                foreach (var implementationTarget in implementationTargetInterfaces)
                {
                    // TODO Here we probably overwrite existing implementations, which would be a hidden conflict.
                    // Maybe enhance the handling and allow multiple impl. but throw error when one of these is resolved..
                    UpdateSingletonInstance(implementationTarget, instance);
                    unresolvedDependencies.Remove(implementationTarget);
                }
            }
            else
            {
                throw new ComponentCreationException("Can not create instance for type '" + type.Name + "', no provider or factory found!");
            }

            return instance;
        }

        /// <summary>
        /// Creates a new instance of a component using the provided factory.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        /// <exception cref="ComponentCreationException"></exception>
        private object CreateInstance(IComponentFactory factory, ISet<Type> unresolvedDependencies, object[] providedParameters = null)
        {
            try
            {
                var instance = factory.CreateInstance(this, unresolvedDependencies, providedParameters);
                return OnPostConstruct(instance, ""); // TODO Component name
            }
            catch (Exception e)
            {
                if (e is CircularDependencyException)
                {
                    throw;
                }
                throw new ComponentCreationException(string.Format("Failed to create instance using component builder '{0}'", factory), e);
            }

        }

        private IEnumerable<Type> FetchImplementationTargets(Type implementation)
        {
            var targets = new List<Type>();

            var customBaseTypes = ReflectionUtil.FindCustomBaseTypes(implementation);

            targets.AddRange(customBaseTypes);

            var customInterfaces = ReflectionUtil.FindCustomInterfaces(implementation);

            targets.AddRange(customInterfaces);

            return targets;
        }

        

        /// <summary>
        /// Returns the instance for a parameter type
        /// </summary>
        /// <param name="contextInfo"></param>
        /// <param name="parameterInfo"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <returns></returns>
        private object ResolveParameterInstance(object contextInfo, ParameterInfo parameterInfo, ISet<Type> unresolvedDependencies)
        {
            object parameter;
            try
            {
                if (unresolvedDependencies.Contains(parameterInfo.ParameterType))
                {
                    throw new CircularDependencyException(contextInfo, parameterInfo.ParameterType);
                }
                var paramInstance = Resolve(parameterInfo.ParameterType, unresolvedDependencies); // Recursive call
                if (paramInstance != null)
                {
                    parameter = paramInstance;
                }
                else
                {
                    throw new AutowireException("Could not resolve parameter " + parameterInfo.Name + " of " + contextInfo + ", the value was (null)!");
                }
            }
            catch (Exception e)
            {
                if (e is CircularDependencyException)
                {
                    throw;
                }
                throw new AutowireException("Autowiring constructor parameter " + parameterInfo.Name + " (" + parameterInfo.ParameterType.Name + ") of " + contextInfo + " has failed!", e);
            }

            return parameter;
        }

        /// <summary>
        /// Find a matching instance for the given parameter
        /// </summary>
        /// <param name="parameterInfo">The requested parameter</param>
        /// <param name="providedParameters">A list of available arguments</param>
        /// <returns></returns>
        private object FindParamInstance(ParameterInfo parameterInfo, object[] providedParameters)
        {
            if(parameterInfo == null) throw new ArgumentNullException("parameterInfo");
            if(providedParameters == null) throw new ArgumentNullException("providedParameters");

            if (providedParameters.Length == 0) return null;
            return (from p in providedParameters
                    where parameterInfo.ParameterType.IsInstanceOfType(p)
                    select p).FirstOrDefault();
        }

        
        /// <summary>
        /// Register the given instance as implemnetation singleton for the given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        private void UpdateSingletonInstance(Type type, object instance)
        {
            if (!_serviceRegistry.ContainsKey(type))
            {
                _serviceRegistry.Add(type, instance);
            }
            else
            {
                _serviceRegistry[type] = instance;
            }
        }

        #endregion


        #region Lifetime Event handlers


        /// <summary>
        /// Occurs when a component has been created.
        /// </summary>
        /// <param name="component"></param>
        private object OnPostConstruct(object component, string name)
        {
            try
            {
                component = OnBeforeInitialisation(component, name);

                // Initialisation callbacks
                component = OnInitialisation(component, name);

                component = OnAfterInitialisation(component, name);

                return component;
            }
            catch (Exception e)
            {
                throw new PostConstructHandlerException(string.Format("Failed to handle post construct lifetime of component '{0}'", component.GetType().Name), e);
            }
        }

        /// <summary>
        /// Occurs when the component should be initialized.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object OnInitialisation(object component, string name)
        {
            // If a method with the [PostConstruct] Attribute is present, execute the method.
            var initMethdo = component.GetType().GetMethods().FirstOrDefault(x => System.Attribute.IsDefined(x, typeof (PostConstructAttribute)));
            if (initMethdo != null)
            {
                initMethdo.Invoke(component, null);
            }

            return component;
        }


        /// <summary>
        /// Occurs when a component has been created and autowired, but before its initialisation call backs.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object OnBeforeInitialisation(object component, string name)
        {
            foreach (var componentPostProcessor in Configuration.AllComponentPostProcessors)
            {
                component = componentPostProcessor.postProcessBeforeInitialisation(component, name);
            }
            return component;
        }


        /// <summary>
        /// Occurs when a component has been created and autowired and initialisation callbacks have run.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private object OnAfterInitialisation(object component, string name)
        {
            foreach (var componentPostProcessor in Configuration.AllComponentPostProcessors)
            {
                component = componentPostProcessor.postProcessAfterInitialisation(component, name);
            }
            return component;
        }


        #endregion

        public override string ToString()
        {
            return ContextName + " holding " + _serviceRegistry.Count + " entries!";
        }
    }
}
