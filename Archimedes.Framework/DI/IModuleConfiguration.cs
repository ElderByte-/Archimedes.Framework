using System;
using Archimedes.Framework.DI.Factories;

namespace Archimedes.Framework.DI
{
    public interface IModuleConfiguration
    {
        IComponentFactory GetFactoryForType(Type type);


        void RegisterSingleton(Type iface, Type implementationType);

        void RegisterSingleton<TInterface, TImplemention>() where TImplemention : TInterface;


        void RegisterFactoryMethod(Type iface, FactoryMethodReference factoryMethodReference);
    }
}
