using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Thrown when there was a an error while handling the post construct hooks.
    /// </summary>
    public class PostConstructHandlerException : ElderBoxException
    {
        public PostConstructHandlerException(string message) : base(message)
        {
        }

        public PostConstructHandlerException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
