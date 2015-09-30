using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.Localisation;
using NUnit.Framework;

namespace Archimedes.Framework.Test.ConfigurationTest
{
    public  class CultureInfoUtilTest
    {
        [TestCase]
        public void TestGetNeutralCulture()
        {
            var neutral = CultureInfoUtil.GetNeutralCulture(CultureInfo.GetCultureInfo("de-DE"));
            Assert.AreEqual(CultureInfo.GetCultureInfo("de"), neutral);
        }

        [TestCase]
        public void TestFindAllSimilarCultures1()
        {

            CultureInfo[] cultures = {CultureInfo.GetCultureInfo("de-DE"), CultureInfo.GetCultureInfo("en") , CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("en-GB") };
            var similar = CultureInfoUtil.FindAllSimilarCultures(cultures, CultureInfo.GetCultureInfo("en-US"));

            Assert.IsTrue(similar.Contains(CultureInfo.GetCultureInfo("en-US")));
            Assert.IsTrue(similar.Contains(CultureInfo.GetCultureInfo("en-GB")));
            Assert.IsTrue(similar.Contains(CultureInfo.GetCultureInfo("en")));
            Assert.IsFalse(similar.Contains(CultureInfo.GetCultureInfo("de-DE")));
        }
    }
}
