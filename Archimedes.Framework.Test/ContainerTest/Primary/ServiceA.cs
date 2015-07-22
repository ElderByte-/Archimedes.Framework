using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest.Primary
{
    [Service]
    class ServiceA
    {
        [Inject]
        private IServiceVV _serviceVV;
    }
}
