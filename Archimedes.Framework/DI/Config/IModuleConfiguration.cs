using System;
using System.Collections.Generic;
using Archimedes.Framework.DI.Config;
using Archimedes.Framework.DI.Factories;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Holds a set of registered types with their assigned factorys to create instances of them. 
    /// </summary>
    public interface IModuleConfiguration
    {
        /// <summary>
        /// Gets all component post processors to apply
        /// </summary>
        List<IComponentPostProcessor> AllComponentPostProcessors { get; }

        /// <summary>
        /// Resolves a component factory for the given type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IComponentFactory GetFactoryForType(Type type);


        /// <summary>
        /// Register an singleton type with its implementation type.
        /// </summary>
        /// <param name="iface"></param>
        /// <param name="implementationType"></param>
        void RegisterSingleton(Type iface, Type implementationType);

        /// <summary>
        /// Register an singleton type with its implementation type.
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplemention"></typeparam>
        void RegisterSingleton<TInterface, TImplemention>() where TImplemention : TInterface;

        /// <summary>
        /// Register a type which will use the provided factory method reference to create the instance.
        /// </summary>
        /// <param name="iface"></param>
        /// <param name="factoryMethodReference"></param>
        void RegisterFactoryMethod(Type iface, FactoryMethodReference factoryMethodReference);
    }
}
