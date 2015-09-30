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
        private readonly IDictionary<CultureInfo, IDictionary<string, string>> _cultureIndex = new Dictionary<CultureInfo, IDictionary<string, string>>();

        /// <summary>
        /// Update the text value of the given text-key and culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Update(CultureInfo culture, string key, string value)
        {
            if(culture == null) throw new ArgumentNullException("culture");
            key = key.ToLower();

            if (!_cultureIndex.ContainsKey(culture))
            {
                _cultureIndex.Add(culture, new Dictionary<string, string>());
            }
            if (_cultureIndex[culture].ContainsKey(key))
            {
                _cultureIndex[culture][key] = value;
            }
            else
            {
                _cultureIndex[culture].Add(key, value);
            }
        }

        /// <summary>
        /// Update the text value of the given text-key and culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="messages"></param>
        public void Update(CultureInfo culture, IDictionary<string, string> messages)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            if (!_cultureIndex.ContainsKey(culture))
            {
                _cultureIndex.Add(culture, new Dictionary<string, string>());
            }

            foreach (var kv in messages)
            {
                var key = kv.Key.ToLower();

                if (_cultureIndex[culture].ContainsKey(key))
                {
                    _cultureIndex[culture][key] = kv.Value;
                }
                else
                {
                    _cultureIndex[culture].Add(key, kv.Value);
                }
            }
        }

        /// <summary>
        /// Clear the cache of the given culture
        /// </summary>
        /// <param name="culture"></param>
        public void Clear(CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            if (_cultureIndex.ContainsKey(culture))
            {
                _cultureIndex.Remove(culture);
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
            key = key.ToLower();

            var translation = FindExact(culture, key);

            if (translation == null && culture.IsNeutralCulture)
            {
                // Maybe we query for a neutral culture, but have only specific ones loaded.
                var cultures = CultureInfoUtil.FindAllSimilarCultures(AllLoadedCultures, culture);
                foreach (var similar in cultures)
                {
                    translation = FindExact(similar, key);
                    if (translation != null) break;
                }
            }

            return translation;
        }

        public IEnumerable<CultureInfo> AllLoadedCultures
        {
            get
            {
                return _cultureIndex.Keys;
            }
        }


        private string FindExact(CultureInfo culture, string key)
        {
            if (culture == null) throw new ArgumentNullException("culture");

            if (_cultureIndex.ContainsKey(culture))
            {
                if (_cultureIndex[culture].ContainsKey(key))
                {
                    return _cultureIndex[culture][key];
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
            return _cultureIndex.ContainsKey(culture);
        }
    }
}
