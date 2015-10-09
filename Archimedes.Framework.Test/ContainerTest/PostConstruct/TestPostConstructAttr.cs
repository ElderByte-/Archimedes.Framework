using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.DI;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ContainerTest.PostConstruct
{
    public class TestPostConstructAttr
    {

        [TestCase]
        public void TestPostConstructAttr1()
        {
            var container = new ElderBox();

            var instance = container.Create<ServiceWithInitialisation>();

            Assert.AreEqual("PostConstruct", instance.Value);
        }
    }
}
