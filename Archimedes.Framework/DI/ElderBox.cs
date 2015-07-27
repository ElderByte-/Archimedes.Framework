using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Archimedes.Framework.Context;
using Archimedes.Framework.ContextEnvironment;
using Archimedes.Framework.DI.Attribute;
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

        /// <summary>
        /// Gets the container configuration
        /// </summary>
        public IModuleConfiguration Configuration
        {
            get { return _configuration;}
        }

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
            return factory.CreateInstance(this, new HashSet<Type>(), args);
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
        /// The name of the context of this container
        /// </summary>
        public string ContextName
        {
            get { return _name; }
        }

        /// <summary>
        /// Auto wire fields / property dependencies which are annotated with <see cref="InjectAttribute"/> 
        /// Usefull if you have already an instance which could not be created using this DI container.
        /// </summary>
        /// <param name="instance"></param>
        [DebuggerStepThrough]
        public void Autowire(object instance)
        {
            Autowire(instance, new HashSet<Type>());
        }

        [Obsolete("Move all register method to configuration, add a InstanceFactoryProvider...")]
        public void RegisterInstance<T>(T serviceInstance)
        {
            UpdateSingletonInstance(typeof(T), serviceInstance);
        }

        #endregion

        #region Private methods

        [DebuggerStepThrough]
        private object Resolve(Type type, HashSet<Type> unresolvedDependencies)
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
                throw new NotSupportedException("Something went wrong while resolving instance for type " + type.Name);
            }

            return instance;
        }

        /// <summary>
        /// Autowires all fields which have the [Inject] Attribute with the resolved dependency.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="unresolvedDependencies"></param>
        [DebuggerStepThrough]
        internal void Autowire(object instance, HashSet<Type> unresolvedDependencies)
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
                            throw new AutowireException("Autowiring of Field " + targetField.Name + "("+targetField.FieldType.Name+") has failed!", e);
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
        /// Autofills all fields of the given instance which have the [Value(...)] atribute.
        /// </summary>
        /// <param name="instance"></param>
        private void AutowireConfiguration(object instance, HashSet<Type> unresolvedDependencies)
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
                            configurator.SetValue(targetField, instance, valueAttr.Expression);
                        }
                    }
                }
            }
        }



        private object ResolveInstanceFor(Type type, HashSet<Type> unresolvedDependencies)
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
                instance = factory.CreateInstance(this, unresolvedDependencies);

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
                throw new AutowireException("Can not create instance for type '" + type.Name + "', no provider or factory found!");
            }

            return instance;
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
        /// Resolves all parameter instances of the given constructor.
        /// </summary>
        /// <param name="contextInfo"></param>
        /// <param name="parameterInfos"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        internal object[] AutowireParameters(object contextInfo, ParameterInfo[] parameterInfos, HashSet<Type> unresolvedDependencies, object[] providedParameters)
        {
            if (parameterInfos == null) throw new ArgumentNullException("parameterInfos");

            var parameters = new List<object>();

            foreach (var parameter in parameterInfos)
            {
                object paramInstance = FindParamInstance(parameter, providedParameters);
                if (paramInstance == null)
                {
                    paramInstance = ResolveParameterInstance(contextInfo, parameter, unresolvedDependencies);
                }
                parameters.Add(paramInstance);
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// Returns the instance for a parameter type
        /// </summary>
        /// <param name="contextInfo"></param>
        /// <param name="parameterInfo"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <returns></returns>
        private object ResolveParameterInstance(object contextInfo, ParameterInfo parameterInfo, HashSet<Type> unresolvedDependencies)
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
                    throw new NotSupportedException("Could not resolve parameter " + parameterInfo.Name + " of " + contextInfo + ", the value was (null)!");
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
        /// <param name="parameterInfo"></param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        private object FindParamInstance(ParameterInfo parameterInfo, object[] providedParameters)
        {
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

        public override string ToString()
        {
            return ContextName + " holding " + _serviceRegistry.Count + " entries!";
        }
    }
}
