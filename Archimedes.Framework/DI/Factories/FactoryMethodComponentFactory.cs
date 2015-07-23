using System;
using System.Collections.Generic;

namespace Archimedes.Framework.DI.Factories
{


    public class FactoryMethodComponentFactory : IComponentFactory
    {
        public object CreateInstance(ElderBox ctx, HashSet<Type> unresolvedDependencies, object[] providedParameters = null)
        {
            throw new NotImplementedException();
        }
    }

}
