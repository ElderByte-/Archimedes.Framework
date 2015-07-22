using System;

namespace Archimedes.Framework.Stereotype
{

    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        private readonly string _name;
        public ComponentAttribute() { }

        public ComponentAttribute(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
