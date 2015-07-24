using System;

namespace Archimedes.Framework.Context.Annotation
{
    /// <summary>
    /// Indicates that the configuration should include a static binding between the given interface-type and implementation-type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ComponentBindingAttribute : Attribute
    {
        private readonly Type _iface;
        private readonly Type _implementation;


        public ComponentBindingAttribute(Type iface, Type implementation)
        {
            _iface = iface;
            _implementation = implementation;
        }

        public Type Iface
        {
            get { return _iface; }
        }

        public Type Implementation
        {
            get { return _implementation; }
        }
    }
}
