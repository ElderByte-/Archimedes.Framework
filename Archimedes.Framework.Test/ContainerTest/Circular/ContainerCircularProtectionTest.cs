using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    public class ContainerCircularProtectionTest
    {
        [TestCase]
        public void TestCircularProtection()
        {
            var container = new ElderBox(GetConfiguration());

            Assert.Throws<CircularDependencyException>(() => container.Resolve<ServiceY1>());
        }

        [TestCase]
        public void TestCircularProtection2()
        {
            var container = new ElderBox(GetConfiguration());

            Assert.Throws<CircularDependencyException>(() => container.Resolve<ServiceY2>());
        }


        [TestCase]
        public void TestCircularProtectionAutowiring()
        {
            var container = new ElderBox(GetConfiguration());

            Assert.Throws<CircularDependencyException>(() => container.Resolve<ServiceA1>());
        }

        private IModuleConfiguration GetConfiguration()
        {
            return ModuleConfigurationBuilder.BuildAutoConfiguration("Archimedes.*");
        }
    }
}
