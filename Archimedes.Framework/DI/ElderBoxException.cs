using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Base exception class for all exceptions which can be thrown by the ElderBox DI Container
    /// </summary>
    public abstract class ElderBoxException : Exception
    {

        public ElderBoxException(string message) : base(message)
        {
        }

        public ElderBoxException(string messgae, Exception cause) 
            : base(messgae, cause)
        {
        }
    }
}
