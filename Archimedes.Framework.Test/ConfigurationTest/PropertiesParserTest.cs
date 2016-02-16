using Archimedes.Framework.ContextEnvironment.Properties;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    public class PropertiesParserTest
    {
        [TestCase("simpleKey=simpleValue", "simpleKey", "simpleValue")]
        [TestCase("simpleKeyNumber=254", "simpleKeyNumber", "254")]
        public void TestParse(string propertyLine, string expectedKey, string expectedValue)
        {
            var source = new PropertiesPropertySource(propertyLine);

            var properties = source.Load();

            Assert.AreEqual(properties.Get(expectedKey), expectedValue);
        }



        [TestCase("keywithComplexVal=Iamsooo;Coold--&&23412+()&ç", "keywithComplexVal", "Iamsooo;Coold--&&23412+()&ç")]
        [TestCase("keyWithValue=TooCool=324;asdf", "keyWithValue", "TooCool=324;asdf")]
        public void TestParse2(string propertyLine, string expectedKey, string expectedValue)
        {
            var source = new PropertiesPropertySource(propertyLine);

            var properties = source.Load();

            Assert.AreEqual(properties.Get(expectedKey), expectedValue);
        }


    }
}
