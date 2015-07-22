using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    [Service]
    public class ServiceA2
    {
        [Inject]
        private ServiceA3 serviceY2;

        public ServiceA2()
        {

        }
    }
}
