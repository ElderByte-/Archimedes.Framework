using System;
using Archimedes.DI.AOP;

namespace Archimedes.Framework.Test.ContainerTest
{
    [Service]
    public class ServiceC
    {
        [Inject]
        public ServiceC(ServiceA serviceA, ServiceB serviceB)
        {
            if(serviceA == null) throw new ArgumentNullException("serviceA");
            if(serviceB == null) throw new ArgumentNullException("serviceB");

            this.serviceA = serviceA;
            this.serviceB = serviceB;
        }

        public ServiceA serviceA;
        public ServiceB serviceB;
    }
}
