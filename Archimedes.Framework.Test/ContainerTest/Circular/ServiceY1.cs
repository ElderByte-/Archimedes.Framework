using Archimedes.DI.AOP;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    [Service]
    public class ServiceY1
    {
        public ServiceY1(ServiceY2 serviceY2)
        {
            
        }
    }


}
