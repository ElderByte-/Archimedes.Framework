using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Archimedes.Framework.DI.Factories
{
    public class FactoryMethod
    {
        private readonly object _instance;
        private readonly MethodInfo _method;

        public FactoryMethod(object instance, MethodInfo method)
        {
            _instance = instance;
            _method = method;
        }

        public object Invoke(params object[] args)
        {
            return _method.Invoke(_instance, args);
        }

        public MethodInfo Method
        {
            get { return _method; }
        }

        public override string ToString()
        {
            return "Factory{" +_instance.GetType().Name + "." + _method.Name + "}";
        }
    }
}
