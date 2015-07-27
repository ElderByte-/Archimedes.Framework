using System;
using System.Threading;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest.Singletons.Abstract
{
    public class AbstractSingletonsTest
    {
        public static long _instanceCount;

        public static void IncrementInstance()
        {
            Interlocked.Increment(ref _instanceCount);
        }

        public static long InstanceCount
        {
            get { return Interlocked.Read(ref _instanceCount); }
        }

        [TestCase]
        public void TestInstanceCounter()
        {
            _instanceCount = 0;
            Assert.AreEqual(0, InstanceCount);

            IncrementInstance();

            Assert.AreEqual(1, InstanceCount);

            var dummy = new ServiceAbstractSingletonImplementation();

            Assert.AreEqual(2, InstanceCount);
        }


        [TestCase]
        public void TestSingletonBehaviour1()
        {
            _instanceCount = 0;
            Assert.AreEqual(0, InstanceCount);

            try
            {
                var context = new ElderBox(GetConfiguration());

                var instance1 = context.Resolve<ServiceAbstractSingletonA>();
                var instance2 = context.Resolve<ServiceAbstractSingletonB>();
                //var instance3 = context.Resolve<ServiceAbstractSingletonImplementation>();



            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }


            Assert.AreEqual(1, InstanceCount, "Must only create a single instance!");
        }


        private IModuleConfiguration GetConfiguration()
        {
            return ModuleConfigurationBuilder.BuildAutoConfiguration("Archimedes.*");
        }

    }
}
