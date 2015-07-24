using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Archimedes.Framework.DI.Factories
{
    public class FactoryMethodReference
    {
        private readonly Type _factoryImpl;
        private readonly MethodInfo _method;

        /// <summary>
        /// Creates a new factory method reference
        /// </summary>
        /// <param name="factoryImpl">The implementation type of the factory</param>
        /// <param name="method">The method in the factory type which creates the desired implementation</param>
        public FactoryMethodReference(Type factoryImpl, MethodInfo method)
        {
            if (factoryImpl == null) throw new ArgumentNullException("factoryImpl");
            if (method == null) throw new ArgumentNullException("method");


            _factoryImpl = factoryImpl;
            _method = method;
        }

        public MethodInfo Method
        {
            get { return _method; }
        }

        public Type FactoryImplementation
        {
            get { return _factoryImpl; }
        }

        public override string ToString()
        {
            return "Factory{" + _factoryImpl.Name + "." + _method.Name + "}";
        }
    }
}
