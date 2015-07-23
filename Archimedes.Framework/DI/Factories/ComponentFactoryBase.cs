using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.DI.Factories
{
    public abstract class ComponentFactoryBase :  IComponentFactory
    {
        private readonly bool _isPrimary;
        private readonly Type[] _primaryForTypes;
        private readonly string _implementationName;

        protected ComponentFactoryBase(string implementationName, bool isPrimary, Type[] primaryForTypes)
        {
            _implementationName = implementationName;
            _isPrimary = isPrimary;
            _primaryForTypes = primaryForTypes;
        }

        public string ImplementationName
        {
            get { return _implementationName; }
        }

        public bool IsPrimary
        {
            get { return _isPrimary; }
        }

        public Type[] PrimaryForTypes
        {
            get { return _primaryForTypes; }
        }



        public abstract object CreateInstance(ElderBox ctx, HashSet<Type> unresolvedDependencies, object[] providedParameters = null);
    }
}
