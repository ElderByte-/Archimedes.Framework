using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.ContextEnvironment.Properties;

namespace Archimedes.Framework.Localisation.MessageSources
{
    public class MemoryMessageSource : IMessageSource
    {
        private readonly IDictionary<CultureInfo, IDictionary<string, string>> _memoryCache = new Dictionary<CultureInfo, IDictionary<string, string>>();

        public MemoryMessageSource()
        {
            
        }


        /// <summary>
        /// Add a property to this memory property source 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MemoryMessageSource AddMessage(CultureInfo culture, string key, string value)
        {
            if (!_memoryCache.ContainsKey(culture))
            {
                _memoryCache.Add(culture, new Dictionary<string, string>());
            }

            if (!_memoryCache[culture].ContainsKey(key))
            {
                _memoryCache[culture].Add(key, value);
            }
            else
            {
                _memoryCache[culture][key] = value;
            }
            return this;
        }


        public PropertyStore Load(CultureInfo culture)
        {
            if (_memoryCache.ContainsKey(culture))
            {
                return new PropertyStore().Merge(_memoryCache[culture]);
            }
            else
            {
                return new PropertyStore();
            }
        }

        public ISet<CultureInfo> GetAvailableCultures()
        {
            return new HashSet<CultureInfo>(_memoryCache.Keys);
        }

    }
}
