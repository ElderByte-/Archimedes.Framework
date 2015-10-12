using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Thrown when creating a component has failed.
    /// </summary>
    public class ComponentCreationException : ElderBoxException
    {
        public ComponentCreationException(string message) : base(message)
        {
        }

        public ComponentCreationException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
