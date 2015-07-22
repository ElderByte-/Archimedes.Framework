using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    [Service]
    public class ServiceY3
    {
        public ServiceY3(ServiceY1 service1)
        {

        }
    }
}
