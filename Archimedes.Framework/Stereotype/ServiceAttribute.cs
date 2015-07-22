using System;

namespace Archimedes.Framework.Stereotype
{
    /// <summary>
    /// Marks the class as a service component
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : ComponentAttribute
    {
        public ServiceAttribute() { }

        public ServiceAttribute(string name) : base(name)
        {
        }

    }
}
