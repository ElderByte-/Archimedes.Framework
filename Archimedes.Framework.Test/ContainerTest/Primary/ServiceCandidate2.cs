using Archimedes.DI.AOP;

namespace Archimedes.Framework.Test.ContainerTest.Primary
{
    [Primary(typeof(IServiceCandidate))]
    [Service]
    public class ServiceCandidate2 : ServiceCandidate1
    {

    }
}
