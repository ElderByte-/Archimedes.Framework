using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest
{
    [Service]
    class ServiceDSimple
    {
        [Inject]
        public ServiceDSimple(ServiceA serviceA, ServiceB serviceB, ServiceY serviceY)
        {
            if(serviceB == null) throw new ArgumentNullException("serviceB");

            this.ServiceB = serviceB;
            this.ServiceY = serviceY;
        }

        public IServiceB ServiceB;
        public ServiceY ServiceY;
    }
}
