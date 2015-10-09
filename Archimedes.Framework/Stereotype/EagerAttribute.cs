using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.Stereotype
{
    /// <summary>
    /// The marked component will be automatically instantiated after the dependency container has been configured.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class EagerAttribute : Attribute
    {

    }
}
