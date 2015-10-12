using System;
using System.Globalization;
using Archimedes.Framework.Context;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest.Values
{
    public class ValueTest
    {
        [TestCase]
        public void TestValue()
        {
            var context = ApplicationContext.Run();

            var configuration = context.Environment.Configuration;
            configuration.Set("test.simpleString", "MyTest");
            configuration.Set("test.simpleNumber", 12.ToString());
            configuration.Set("test.nullableNumber", 33.ToString());

            var date = DateTime.Now;
            configuration.Set("test.simpleDate", date.ToString(CultureInfo.InvariantCulture));

            var container = context.Container;
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

        [TestCase]
        public void TestBooleanValueInjected()
        {
            var context = new TestApplicationContext();

            var configuration = context.Environment.Configuration;
            configuration.Set("test.simpleBool", "True");
            configuration.Set("test.simpleBoolNegative", "False");
            configuration.Set("test.simpleBool2", "true");
            configuration.Set("test.simpleBoolNegative2", "false");
            configuration.Set("test.simpleBoolOpt", "true");
            configuration.Set("test.simpleBoolNegativeOpt", "false");


            context.Start();


            var valueService = context.Container.Resolve<TestValueService>();

            Assert.AreEqual(false, valueService.BoolUnset);
            Assert.AreEqual(true, valueService.SimpleBool);
            Assert.AreEqual(true, valueService.SimpleBool2);
            Assert.AreEqual(false, valueService.SimpleBoolNegative);
            Assert.AreEqual(false, valueService.SimpleBoolNegative2);
            Assert.AreEqual(true, valueService.SimpleBoolOpt);
            Assert.AreEqual(false, valueService.SimpleBoolNegativeOpt);
            Assert.AreEqual(null, valueService.BoolUnsetOpt);
        }
        
    }
}
