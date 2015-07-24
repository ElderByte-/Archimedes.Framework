using Archimedes.Framework.AOP;
using Archimedes.Framework.Context;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest.Circular
{
    public class ContainerCircularProtectionTest
    {
        [TestCase]
        [ExpectedException(typeof(CircularDependencyException))]
        public void TestCircularProtection()
        {
            var container = new ElderBox(GetConfiguration());

            container.Resolve<ServiceY1>();
        }

        [TestCase]
        [ExpectedException(typeof(CircularDependencyException))]
        public void TestCircularProtection2()
        {
            var container = new ElderBox(GetConfiguration());

            container.Resolve<ServiceY2>();
        }


        [TestCase]
        [ExpectedException(typeof(CircularDependencyException))]
        public void TestCircularProtectionAutowiring()
        {
            var container = new ElderBox(GetConfiguration());

            var service = container.Resolve<ServiceA1>();

            var s2 = service;
        }

        private IModuleConfiguration GetConfiguration()
        {
            return ModuleConfigurationBuilder.BuildAutoConfiguration("Archimedes.*");
        }
    }
}
