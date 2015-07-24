using System.Threading;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Context;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest.Singletons
{
    public class SingletonsTest
    {
        public static long _instanceCount;

        public static void IncrementInstance()
        {
            Interlocked.Increment(ref _instanceCount);
        }


        [TestCase]
        public void TestSingletonBehaviour1()
        {
            var context = new ElderBox(GetConfiguration());

            var instance1 = context.Resolve<ServiceSingletonA>();
            var instance2 = context.Resolve<ServiceSingletonB>();

            long actualInstances = Interlocked.Read(ref _instanceCount);
            Assert.AreEqual(1, actualInstances, "Must only create a single instance!");
        }

        /*
        [TestCase]
        public void TestSingletonBehaviour()
        {
            var context = new ElderBox(GetConfiguration());

            var instance1 = context.Resolve<ServiceSingletonB>();
            var instance2 = context.Resolve<ServiceSingletonC>();

            long actualInstances = Interlocked.Read(ref _instanceCount);
            Assert.AreEqual(1, actualInstances, "Must only create a single instance!");
        }*/


        private ComponentRegisterer GetConfiguration()
        {
            return new ComponentRegisterer(ApplicationContext.Instance.ScanComponents("Archimedes.*"));
        }
    }
}
