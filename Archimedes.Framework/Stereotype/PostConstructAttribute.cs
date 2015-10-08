using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.Stereotype
{
    /// <summary>
    /// Marks a method to be automatically executed after the container is ready
    /// and the component has been created.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PostConstructAttribute : Attribute
    {

    }
}
