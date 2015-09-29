using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.Localisation
{
    /// <summary>
    /// Simple index for culture-key mapped string values.
    /// </summary>
    class TranslationCache
    {
        private readonly IDictionary<string, IDictionary<string, string>> _cultureIndex = new Dictionary<string, IDictionary<string, string>>();

        /// <summary>
        /// Update the text value of the given text-key and culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Update(CultureInfo culture, string key, string value)
        {
            var mculture = CultureToKey(culture);

            if (!_cultureIndex.ContainsKey(mculture))
            {
                _cultureIndex.Add(mculture, new Dictionary<string, string>());
            }
            if (_cultureIndex[mculture].ContainsKey(key))
            {
                _cultureIndex[mculture][key] = value;
            }
            else
            {
                _cultureIndex[mculture].Add(key, value);
            }
        }

        /// <summary>
        /// Update the text value of the given text-key and culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="messages"></param>
        public void Update(CultureInfo culture, IDictionary<string, string> messages)
        {
            var mculture = CultureToKey(culture);

            if (!_cultureIndex.ContainsKey(mculture))
            {
                _cultureIndex.Add(mculture, new Dictionary<string, string>());
            }

            foreach (var kv in messages)
            {
                if (_cultureIndex[mculture].ContainsKey(kv.Key))
                {
                    _cultureIndex[mculture][kv.Key] = kv.Value;
                }
                else
                {
                    _cultureIndex[mculture].Add(kv.Key, kv.Value);
                }
            }
        }

        /// <summary>
        /// Clear the cache of the given culture
        /// </summary>
        /// <param name="culture"></param>
        public void Clear(CultureInfo culture)
        {
            var mculture = CultureToKey(culture);

            if (_cultureIndex.ContainsKey(mculture))
            {
                _cultureIndex.Remove(mculture);
            }
        }

        /// <summary>
        /// Clear the whole cache
        /// </summary>
        public void Clear()
        {
            _cultureIndex.Clear();
        }

        /// <summary>
        /// Returns the value of the given text-key in the given culture if available.
        /// If not found, returns NULL
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Find(CultureInfo culture, string key)
        {
            var mculture = CultureToKey(culture);
            if (_cultureIndex.ContainsKey(mculture))
            {
                if (_cultureIndex[mculture].ContainsKey(key))
                {
                    return _cultureIndex[mculture][key];
                }
            }
            return null;
        }

        /// <summary>
        /// Does the given culture exist in this cache? 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public bool Exists(CultureInfo culture)
        {
            return _cultureIndex.ContainsKey(CultureToKey(culture));
        }

        private string CultureToKey(CultureInfo culture)
        {
            if (culture == null) return "default";
            return culture.Name.ToLower();
        }
    }
}
