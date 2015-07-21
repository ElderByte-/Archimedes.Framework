using Archimedes.DI.AOP;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    [Service]
    public class ServiceY2
    {
        public ServiceY2(ServiceY3 serviceY2)
        {

        }
    }
}
