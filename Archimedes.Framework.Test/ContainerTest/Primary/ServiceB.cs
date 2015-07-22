using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.Primary
{
    [Service]
    public class ServiceB
    {
        [Inject]
        private IServiceCandidate _service;
    }
}
