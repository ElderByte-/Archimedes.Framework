using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    [Service]
    public class ServiceA1
    {
        [Inject]
        private ServiceA2 serviceA2;

        public ServiceA1()
        {
            
        }
    }


}
