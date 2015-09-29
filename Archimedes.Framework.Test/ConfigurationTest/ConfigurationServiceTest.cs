using Archimedes.Framework.ContextEnvironment;
using Archimedes.Framework.ContextEnvironment.Properties;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    class ConfigurationServiceTest
    {
        [TestCase]
        public void TestLoadHidden()
        {
            var configurationService = new EnvironmentService();

            var memoryConfiguration = new MemoryPropertySource()
                .AddProperty("test.password.$hidden", "MTMzN2wzM3Q=");

            configurationService.PropertySources.Add(memoryConfiguration);
            configurationService.Refresh();


            Assert.AreEqual("1337l33t", configurationService.Configuration.Get("test.password"));
        }
    }
}
