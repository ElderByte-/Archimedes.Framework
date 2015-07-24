using System;
using System.Collections.Generic;

namespace Archimedes.Framework.DI.Factories
{


    public class FactoryMethodComponentFactory : ComponentFactoryBase
    {
        private readonly FactoryMethodReference _factoryMethodReference;

        public FactoryMethodComponentFactory(FactoryMethodReference factoryMethodReference, string implementationName, bool isPrimary, Type[] primaryForTypes)
            : base(implementationName, isPrimary, primaryForTypes)
        {
            _factoryMethodReference = factoryMethodReference;
        }


        public override object CreateInstance(ElderBox ctx, HashSet<Type> unresolvedDependencies, object[] providedParameters = null)
        {
            var factoryInstance = ctx.Resolve(_factoryMethodReference.FactoryImplementation);

            var parameters = ctx.AutowireParameters(null, _factoryMethodReference.Method.GetParameters(), unresolvedDependencies, providedParameters);

            return _factoryMethodReference.Method.Invoke(factoryInstance, parameters);
        }

        public override string ToString()
        {
            return _factoryMethodReference.ToString();
        }
    }

}
