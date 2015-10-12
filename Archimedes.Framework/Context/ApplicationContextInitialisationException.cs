using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.Context
{
    /// <summary>
    /// Thrown when the initialisation of the application context failed.
    /// </summary>
    public class ApplicationContextInitialisationException : ApplicationContextException
    {
        public ApplicationContextInitialisationException(string message) : base(message)
        {
        }

        public ApplicationContextInitialisationException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
