using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ContainerTest
{
    [Service]
    public class ServiceAmbigOne : IServiceAmbig
    {
        public void Test()
        {
        }
    }

    [Service]
    public class ServiceAmbigTwo : IServiceAmbig
    {
        public void Test()
        {
        }
    }


    public interface IServiceAmbig
    {
        void Test();
    }
}
