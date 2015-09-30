using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archimedes.Framework.Localisation
{
    public static class CultureInfoUtil
    {


        /// <summary>
        /// Returns the neutral culture of the given one.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static CultureInfo GetNeutralCulture(CultureInfo culture)
        {
            if (CultureInfo.InvariantCulture.Equals(culture)) return null;
            if (culture.IsNeutralCulture) return culture;
            return GetNeutralCulture(culture.Parent);
        }

        public static ISet<CultureInfo> FindAllSimilarCultures(IEnumerable<CultureInfo> cultures, CultureInfo needle)
        {
            var similars = new HashSet<CultureInfo>();

            needle = GetNeutralCulture(needle);

            foreach (var potential in cultures)
            {
                var basePotential = GetNeutralCulture(potential);
                if (basePotential != null && basePotential.Equals(needle))
                {
                    similars.Add(potential);
                }
            }
            return similars;
        } 



    }
}
