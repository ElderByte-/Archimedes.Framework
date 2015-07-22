using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace Archimedes.Framework.ContextEnvironment.Properties
{
    public class PropertiesPropertySource : IPropertySource
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex KeyValueParser = new Regex("(.*?)=(.*)");
        private readonly string _rawPropertiesData;

        /// <summary>
        /// Creates a new PropertiesPropertySource with the raw properties data.
        /// It will be parsed on load into a property store.
        /// </summary>
        /// <param name="rawPropertiesData"></param>
        public PropertiesPropertySource(string rawPropertiesData)
        {
            if (rawPropertiesData == null) throw new ArgumentNullException("rawPropertiesData");

            _rawPropertiesData = rawPropertiesData;
        }

        public PropertyStore Load()
        {
            var properties = new PropertyStore();

            var tmp = Parse(_rawPropertiesData);
            properties.Merge(tmp);

            return properties;
        }

        #region Private methods

        private Dictionary<string, string> Parse(string rawpropertyData)
        {
            string[] lines = Regex.Split(rawpropertyData, @"\r?\n|\r");
            return Parse(lines);
        }

        private Dictionary<string, string> Parse(string[] propertyLines)
        {
            var properties = new Dictionary<string, string>();


            foreach (var propertyLine in propertyLines)
            {
                var parsed = ParseLine(propertyLine);

                if (parsed != null)
                {
                    if (!properties.ContainsKey(parsed.Item1))
                    {
                        properties.Add(parsed.Item1, parsed.Item2);
                    }
                    else
                    {
                        properties[parsed.Item1] = parsed.Item2;
                        Log.Warn("Property key '" + parsed.Item1 + "' is defined multiple times. Value got overriden.");
                    }
                }
            }
            return properties;
        }

        private Tuple<string, string> ParseLine(string propertyLine)
        {
            if (!propertyLine.Trim().StartsWith("#"))
            {
                if (KeyValueParser.IsMatch(propertyLine))
                {
                    var key = KeyValueParser.Match(propertyLine).Groups[1].Value;
                    var value = KeyValueParser.Match(propertyLine).Groups[2].Value;

                    return new Tuple<string, string>(key, value);
                }
            }
            return null;
        }

        #endregion
    }
}
