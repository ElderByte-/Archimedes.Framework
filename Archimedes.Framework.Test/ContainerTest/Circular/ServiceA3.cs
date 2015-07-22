using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    [Service]
    public class ServiceA3
    {
        [Inject]
        private ServiceA1 service1;

        public ServiceA3()
        {

        }
    }
}
