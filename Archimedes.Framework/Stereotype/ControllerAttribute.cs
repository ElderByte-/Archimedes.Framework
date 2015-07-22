using System;

namespace Archimedes.Framework.Stereotype
{
    /// <summary>
    /// Marks the component as a controller
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : ComponentAttribute
    {
        public ControllerAttribute() { }

        public ControllerAttribute(string name)
            : base(name)
        {
        }
    }
}
