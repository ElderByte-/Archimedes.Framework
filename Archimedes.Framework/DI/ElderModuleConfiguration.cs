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
    public class ElderModuleConfiguration : IModuleConfiguration
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<Type, ImplementationRegistry> _componentRegistry = new Dictionary<Type, ImplementationRegistry>();

        #endregion

        #region Public methods


        public IComponentFactory GetFactoryForType(Type type)
        {
            return GetImplementaionTypeFor(type);
        }


        /// <summary>
        /// Registers a component as a singleton.
        /// </summary>
        public void RegisterSingleton<TInterface, TImplemention>()
            where TImplemention : TInterface
        {
            RegisterSingleton(typeof(TInterface), typeof(TImplemention));
        }

        /// <summary>
        /// Registers a component as a singleton.
        /// </summary>
        public void RegisterSingleton(Type iface, Type implementationType)
        {
            if (CanCreateInstance(implementationType))
            {
                if (AOPUitl.IsTypeComponent(implementationType))
                {
                    RegisterFactory(iface, FromComponentType(implementationType));
                }
                else
                {
                    RegisterFactory(iface, new TypeComponentFactory(implementationType, null, false, null));
                }
            }
            else
            {
                throw new NotSupportedException(String.Format("Can not create an instance of type {0}", implementationType));
            }
        }

        public void RegisterFactoryMethod(Type iface, FactoryMethodReference factoryMethodReference)
        {
            var factory = new FactoryMethodComponentFactory(factoryMethodReference, null, false, null);
            RegisterFactory(iface, factory);
        }

        #endregion

        #region Private methods

        private void RegisterFactory(Type iface, IComponentFactory factory)
        {
            if (!_componentRegistry.ContainsKey(iface))
            {
                _componentRegistry.Add(iface, new ImplementationRegistry(iface));
            }

            _componentRegistry[iface].Register(factory);
        }

        private TypeComponentFactory FromComponentType(Type implementationType)
        {
            var component = AOPUitl.GetComponentAttribute(implementationType);
            var implementationName = component.Name;

            bool isPrimary = false;
            Type[] primaryForTypes = null;


            var primary = AOPUitl.GetPrimaryAttribute(implementationType);
            if (primary != null)
            {
                isPrimary = true;
                primaryForTypes = primary.PrimaryForTypes;
            }

            return new TypeComponentFactory(implementationType, implementationName, isPrimary, primaryForTypes);
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

        private IComponentFactory GetImplementaionTypeFor(Type type)
        {
            if (_componentRegistry.ContainsKey(type))
            {
                return _componentRegistry[type].TryGetImplementation();
            }
            return null;
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

        #endregion
    }
}
