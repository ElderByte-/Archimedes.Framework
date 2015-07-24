using Archimedes.Framework.AOP;
using Archimedes.Framework.Context;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest.Primary
{
    class PrimaryHandlingTest
    {
        [TestCase]
        public void TestPrimary()
        {
            var context = new ElderBox(GetConfiguration());
            var serviceA = context.Resolve<ServiceA>();
        }

        [TestCase]
        public void TestPrimaryScoped()
        {
            var context = new ElderBox(GetConfiguration());
            var serviceA = context.Resolve<ServiceB>();
        }

        private ComponentRegisterer GetConfiguration()
        {
            return new ComponentRegisterer(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
        }
    }
}
