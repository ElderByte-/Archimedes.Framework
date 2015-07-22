using System;

namespace Archimedes.Framework.ContextEnvironment
{
    public class UnknownParameterException : Exception
    {
        public UnknownParameterException(string parameter)
            : base("The requiered parameter '" + parameter  + "' was not specified in the App settings!")
        {
            
        }
    }
}
 