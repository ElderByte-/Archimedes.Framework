using System;
using System.Globalization;
using Archimedes.Framework.Configuration;
using Archimedes.Framework.Context;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    public class ValueTest
    {
        [TestCase]
        public void TestValue()
        {
            ApplicationContext.Instance.EnableAutoConfiguration();

            var configuration = ApplicationContext.Instance.Environment.Configuration;
            configuration.Set("test.simpleString", "MyTest");
            configuration.Set("test.simpleNumber", 12.ToString());
            configuration.Set("test.nullableNumber", 33.ToString());

            var date = DateTime.Now;
            configuration.Set("test.simpleDate", date.ToString(CultureInfo.InvariantCulture));

            var container = ApplicationContext.Instance.Container;
            var valueService = container.Resolve<ServiceConfigTarget>();

            Assert.AreEqual("MyTest", valueService.simpleStringValue);
            Assert.AreEqual(12, valueService.simpleIntValue);
            Assert.AreEqual(33, valueService.nullableNumber);


            Assert.AreEqual(date.Year, valueService.simpleDate.Year);
            Assert.AreEqual(date.Month, valueService.simpleDate.Month);
            Assert.AreEqual(date.Day, valueService.simpleDate.Day);
            Assert.AreEqual(date.Hour, valueService.simpleDate.Hour);
            Assert.AreEqual(date.Minute, valueService.simpleDate.Minute);
            Assert.AreEqual(date.Second, valueService.simpleDate.Second);

        }
        
    }
}
