using System;
using System.Diagnostics;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest
{
    [Service]
    public class ServiceD
    {
        [Inject]
        [DebuggerStepThrough]
        public ServiceD(ServiceA serviceA, IServiceB serviceB, ServiceY serviceY)
        {
            if(serviceA == null) throw new ArgumentNullException("serviceA");
            if(serviceB == null) throw new ArgumentNullException("serviceB");

            this.serviceA = serviceA;
            this.serviceB = serviceB;
            this.ServiceY = serviceY;
        }

        public ServiceA serviceA;
        public IServiceB serviceB;
        public ServiceY ServiceY;
    }
}
