using System;
using System.Collections.Generic;

namespace Archimedes.Framework.DI.Factories
{


    public class FactoryMethodComponentFactory : ComponentFactoryBase
    {
        private readonly FactoryMethod _factoryMethod;

        public FactoryMethodComponentFactory(FactoryMethod factoryMethod, string implementationName, bool isPrimary, Type[] primaryForTypes)
            : base(implementationName, isPrimary, primaryForTypes)
        {
            _factoryMethod = factoryMethod;
        }


        public override object CreateInstance(ElderBox ctx, HashSet<Type> unresolvedDependencies, object[] providedParameters = null)
        {
            var parameters = ctx.AutowireParameters(null, _factoryMethod.Method.GetParameters(), unresolvedDependencies, providedParameters);
            return _factoryMethod.Invoke(parameters);
        }

        public override string ToString()
        {
            return _factoryMethod.ToString();
        }
    }

}
