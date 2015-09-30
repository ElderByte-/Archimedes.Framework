using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Archimedes.Framework.Localisation;
using Archimedes.Framework.Localisation.MessageSources;
using NUnit.Framework;

namespace Archimedes.Framework.Test.Localisation
{
    public class LocalisationServiceTest
    {
        [TestCase("en-US", "simple.test", "Hello world!")]
        [TestCase("en-US", "simple.test.gen", "Gun Bun!")]
        [TestCase("de", "simple.test", "Hallo Welt!")]
        [TestCase("en", "simple.test", "Hello world!")]
        [TestCase("en-US", "simple.test.only.invariant", "I only exist in default!")]
        public void TestTranslation(string cultureStr, string key, string expectedTranslation)
        {
            var localisationService = Init();
            var culture = CultureInfo.GetCultureInfo(cultureStr);
            Assert.AreEqual(expectedTranslation, localisationService.GetTranslation(culture, key));
        }

        private ILocalisationService Init()
        {
            var messages = new MemoryMessageSource();

            messages
                .AddMessage(new CultureInfo("en-US"), "simple.test", "Hello world!")
                .AddMessage(new CultureInfo("en-US"), "simple.test.2", "nice")
                .AddMessage(new CultureInfo("en"), "simple.test.gen", "Gun Bun!")
                .AddMessage(new CultureInfo("de"), "simple.test", "Hallo Welt!")
                .AddMessage(CultureInfo.InvariantCulture, "simple.test.only.invariant", "I only exist in default!")
                ;

            return new LocalisationService(messages);
        }


    }
}
