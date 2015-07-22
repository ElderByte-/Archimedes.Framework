using System;
using Archimedes.Framework.DI.Attribute;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    [Service]
    internal class ServiceConfigTarget
    {
        [Value("${test.simpleString}")]
        public string simpleStringValue;

        [Value("${test.simpleNumber}")]
        public int simpleIntValue;

        [Value("${test.nullableNumber}")]
        public int? nullableNumber;

        [Value("${test.simpleDate}")]
        public DateTime simpleDate;
    }
}
