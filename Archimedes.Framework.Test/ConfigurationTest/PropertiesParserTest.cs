using Archimedes.Framework.Configuration;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    public class PropertiesParserTest
    {
        [TestCase("simpleKey=simpleValue", "simpleKey", "simpleValue")]
        [TestCase("simpleKeyNumber=254", "simpleKeyNumber", "254")]
        [TestCase("keywithComplexVal=Iamsooo;Coold--&&23412+()&ç", "keywithComplexVal", "Iamsooo;Coold--&&23412+()&ç")]
        [TestCase("keyWithValue=TooCool=324;asdf", "keyWithValue", "TooCool=324;asdf")]

        public void TestParse(string propertyLine, string expectedKey, string expectedValue)
        {
            var result = PropertiesFileParser.ParseLine(propertyLine);

            Assert.AreEqual(expectedKey, result.Item1);
            Assert.AreEqual(expectedValue, result.Item2);

        }



    }
}
