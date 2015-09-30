using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.ContextEnvironment.Properties;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    public class CommandLinePropertySourceTest
    {
        [TestCase]
        public void Test()
        {
            string[] arguments = {"/my.life", "/erp.name", "HuluMulu", "/filepath",@"C:\my folder with spaces\and some.txt", "/quiet"};
            var source = new CommandLinePropertySource(arguments);
            var store = source.Load();


            Assert.AreEqual(true, store.IsTrue("quiet"));
            Assert.AreEqual("HuluMulu", store.Get("erp.name"));
            Assert.AreEqual(@"C:\my folder with spaces\and some.txt", store.Get("filepath"));
            Assert.AreEqual(true, store.IsTrue("my.life"));

            Assert.AreEqual(false, store.IsTrue("i.do.not.exist.at.all"));
            Assert.AreEqual(false, store.GetOptional("i.do.not.exist.at.all").IsPresent);
        }


        [TestCase]
        public void TestRealScenario()
        {
            string[] arguments = { "/erp.connection", @"C:\Temp\Planung\DATA-007_Zu" };
            var source = new CommandLinePropertySource(arguments);
            var store = source.Load();

            Assert.AreEqual(@"C:\Temp\Planung\DATA-007_Zu", store.Get("erp.connection"));

            Assert.AreEqual(false, store.IsTrue("i.do.not.exist.at.all"));
            Assert.AreEqual(false, store.GetOptional("i.do.not.exist.at.all").IsPresent);
        }

    }
}
