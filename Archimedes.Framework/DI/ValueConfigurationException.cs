using System;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Thrown when configuring a value has failed
    /// </summary>
    public class ValueConfigurationException : Exception
    {

        public ValueConfigurationException(string message, Exception cause)
            : base(message, cause)
        {
            
        }

    }
}
