using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Archimedes.Framework.AOP;
using Archimedes.Framework.DI.Factories;
using log4net;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Base class of the ElderBox DI configuration
    /// </summary>
    public abstract class ElderModuleConfiguration : IModuleConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<Type, ImplementationRegistry> _componentRegistry = new Dictionary<Type, ImplementationRegistry>();


        /// <summary>
        /// Called before the dependency container is being constructed.
        /// 
        /// Sub classes are expected to overwrite this method and 
        /// call <see cref="RegisterSingleton"/> to configure the module.
        /// </summary>
        public void Configure()
        {
            ConfigureInternal();
            LogConfiguration();
        }


        public abstract void ConfigureInternal();



        public IComponentFactory GetFactoryForType(Type type)
        {
            var implementationType = GetImplementaionTypeFor(type) ?? type;

            ThrowIfTypeNotComponent(implementationType);

            if (CanCreateInstance(implementationType))
            {

                return new TypeComponentFactory(implementationType);
            }
            else
            {
                throw new NotSupportedException(string.Format("Can not create an instance of type {0}", implementationType));
            }
        }

        /// <summary>
        /// Is it possible to create an instance of the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CanCreateInstance(Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }

        [DebuggerStepThrough]
        private Type GetImplementaionTypeFor(Type type)
        {
            if (_componentRegistry.ContainsKey(type))
            {
                return _componentRegistry[type].TryGetImplementation();
            }
            return null;
        }



        /// <summary>
        /// Registers a component as a singleton.
        /// </summary>
        protected void RegisterSingleton<TInterface, TImplemention>()
            where TImplemention : TInterface
        {
            RegisterSingleton(typeof(TInterface), typeof(TImplemention));
        }

        /// <summary>
        /// Registers a component as a singleton.
        /// </summary>
        protected void RegisterSingleton(Type iface, Type impl)
        {
            if (!_componentRegistry.ContainsKey(iface))
            {
                _componentRegistry.Add(iface, new ImplementationRegistry(iface));
            }

            _componentRegistry[iface].Register(impl);
        }


        [DebuggerStepThrough]
        private void ThrowIfTypeNotComponent(Type type)
        {
            if (!AOPUitl.IsTypeComponent(type)) throw new AutowireException("The implementation " + type + " is not marked as Component and can therefore not be used." +
                                                                              " Did you forget to add a [Service] or [Controller] annotation?");
        }

        

        private void LogConfiguration()
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug("DI Module Configuration:");
                foreach (var kv in _componentRegistry)
                {
                    Log.Debug(kv.Key + "  ===>  " + kv.Value);
                }
            }
        }
    }
}
