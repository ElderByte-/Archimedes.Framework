using Archimedes.DI.AOP;

namespace Archimedes.Framework.Test.ContainerTest
{
    [Service]
    public class ServiceY
    {
         [Inject]
        public ServiceY(IServiceB serviceB)
        {

        }
    }
}
