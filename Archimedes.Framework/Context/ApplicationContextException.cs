using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.Context
{
    /// <summary>
    /// Base exception for all application context exceptions
    /// </summary>
    public class ApplicationContextException : Exception
    {
        public ApplicationContextException(string message) : base(message)
        {
        }

        public ApplicationContextException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
