using System.Linq;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Context;
using Archimedes.Framework.DI;
using Archimedes.Framework.Stereotype;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest
{
    public class ContainerTest
    {
        [Inject] private ServiceA _serviceA;

        [Inject] private IServiceB _serviceB;

        [TestCase]
        public void TestComponentScan()
        {
            var componentScanner = ComponentUtil.BuildComponentScanner();
            var components = componentScanner.ScanByAttribute("Archimedes.*").ToList();

            Assert.True(components.Contains(typeof (ServiceA)), "");
            Assert.True(components.Contains(typeof (ServiceB)), "");
            Assert.True(components.Contains(typeof (ServiceC)), "");
            Assert.True(components.Contains(typeof (ServiceD)), "");
        }


        [TestCase]
        public void TestAutoConfiguration()
        {
            var componentScanner = ComponentUtil.BuildComponentScanner();
            var components = componentScanner.ScanByAttribute("Archimedes.*").ToList();

            var conf = new ComponentRegisterer(new ElderModuleConfiguration());
            conf.RegisterComponents(components);
        }


        [TestCase]
        public void TestSimpleConstructorWiring()
        {
            var context = new ElderBox(GetConfiguration());
            var instance = context.Resolve<ServiceC>();
        }


        [TestCase]
        public void TestSimpleInstanceWiring()
        {

            var context = new ElderBox(GetConfiguration());


            context.Autowire(this);

            Assert.IsNotNull(_serviceA, "Failed to autowire instance!");
            Assert.IsNotNull(_serviceB, "Failed to autowire instance!");
        }


        [TestCase]
        public void TestInterfaceConstructorWiringServiceY()
        {
            var context = new ElderBox(GetConfiguration());
            var instance = context.Resolve<ServiceY>();
        }

        [TestCase]
        public void TestInterfaceConstructorWiringServiceB()
        {
            var context = new ElderBox(GetConfiguration());
            var instance = context.Resolve<ServiceB>();
        }

        [TestCase]
        public void TestInterfaceConstructorWiringSimple()
        {
            var context = new ElderBox(GetConfiguration());
            var instance = context.Resolve<ServiceDSimple>();
        }

        [TestCase]
        public void TestInterfaceConstructorWiring()
        {
            var context = new ElderBox(GetConfiguration());
            var instance = context.Resolve<ServiceD>();
        }


        [TestCase]
        public void TestAmbigous()
        {
            var context = new ElderBox(GetConfiguration());

            Assert.Throws<AmbiguousMappingException>(() => context.Resolve<IServiceAmbig>());
        }

        [TestCase]
        public void TestConstructorByPassing()
        {
            var context = new ElderBox(GetConfiguration());

            var imp = context.Resolve<ServiceX>();
        }

        private IModuleConfiguration GetConfiguration()
        {
            return ModuleConfigurationBuilder.BuildAutoConfiguration("Archimedes.*");
        }

    }
}
