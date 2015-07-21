using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.Configuration.Properties
{
    /// <summary>
    /// A memory property source is usefull for runtime configuration / in code configuration
    /// </summary>
    public class MemoryPropertySource : IPropertySource
    {
        private readonly IDictionary<string, string> _keyValuePairs;


        public MemoryPropertySource(IDictionary<string,string> keyValuePairs)
        {
            _keyValuePairs = keyValuePairs;
        }

        public MemoryPropertySource()
            :this(new Dictionary<string, string>())
        {
        }

        /// <summary>
        /// Add a property to this memory property source 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public MemoryPropertySource AddProperty(string key, string value)
        {
            if (!_keyValuePairs.ContainsKey(key))
            {
                _keyValuePairs.Add(key, value);
            }
            else
            {
                _keyValuePairs[key] = value;
            }

            return this;
        }


        public PropertyStore Load()
        {
            return new PropertyStore().Merge(_keyValuePairs);
        }
    }
}
