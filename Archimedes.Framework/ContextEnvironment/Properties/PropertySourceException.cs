using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.ContextEnvironment.Properties
{
    /// <summary>
    /// Thrown when there was a problem with a property source
    /// </summary>
    public class PropertySourceException : Exception
    {
        public PropertySourceException(string message) : base(message)
        {
            
        }

        public PropertySourceException(string message, Exception cause)
            : base(message, cause)
        {

        }
    }
}
