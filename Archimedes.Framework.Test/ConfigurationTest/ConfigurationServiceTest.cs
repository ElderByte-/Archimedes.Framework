using Archimedes.Framework.Configuration;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    class ConfigurationServiceTest
    {
        [TestCase()]
        public void TestLoadHidden()
        {
            var configurationService = new ConfigurationService();

            configurationService.LoadConfiguration("/test.password.$hidden", "MTMzN2wzM3Q="); // base64("1337l33t")
            Assert.AreEqual("1337l33t", configurationService.Configuration.Get("test.password"));
        }
    }
}
