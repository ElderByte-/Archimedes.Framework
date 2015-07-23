using System;
using Archimedes.Framework.DI.Factories;

namespace Archimedes.Framework.DI
{
    public interface IModuleConfiguration
    {
        void Configure();


        IComponentFactory GetFactoryForType(Type type);
    }
}
