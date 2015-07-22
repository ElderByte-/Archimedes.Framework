using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Archimedes.Framework.ContextEnvironment.Properties;
using Archimedes.Patterns.Utils;

namespace Archimedes.Framework.ContextEnvironment
{
    /// <summary>
    /// Holds the global configuration of this application.
    /// </summary>
    public class EnvironmentService : IEnvironmentService
    {
        #region Fields

        private const string PropertiesFileName = "application.properties";
        private readonly PropertyStore _configuration = new PropertyStore();
        private readonly List<IPropertySource> _propertySources = new List<IPropertySource>();

        #endregion

        /// <summary>
        /// Creates a new ConfigurationService
        /// </summary>
        public EnvironmentService()
        {
            if (Assembly.GetEntryAssembly() != null) // Avoid accessing assembly incase of unit tests or similar
            {   
                PropertySources.Add(new PropertiesPropertySource(AppUtil.ApplicaitonBinaryFolder + @"\" + PropertiesFileName));
                PropertySources.Add(new PropertiesPropertySource(AppUtil.AppDataFolder + @"\" + PropertiesFileName));
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets the configuration properties
        /// </summary>
        public PropertyStore Configuration
        {
            get { return _configuration; }
        }

        /// <summary>
        /// Provides access to the property sources which are configured in this environment.
        /// The order of the property source are very important, in case of conflicts the properties
        /// will be overridden.
        /// 
        /// The last property source in this list has the highest proirity.
        /// </summary>
        public List<IPropertySource> PropertySources
        {
            get { return _propertySources; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Loads all properties
        /// </summary>
        public void Refresh()
        {
            foreach (var source in PropertySources)
            {
                var properties = source.Load();

                InterpretProperties(properties);

                Configuration.Merge(properties);
            }
        }

        #endregion

        #region Private methods

        private void InterpretProperties(PropertyStore properties)
        {
            var allProperties = properties.ToKeyValuePairs();
            foreach (var key in allProperties.Keys)
            {
                if (key.Contains(".$hidden"))
                {
                    var hiddenValue = properties.Get(key);
                    properties.Set(key.Replace(".$hidden", ""), Unhide(hiddenValue));
                }
            }
        }

        private string Unhide(string hidden)
        {
            if (!string.IsNullOrEmpty(hidden))
            {
                byte[] decodedBytes = Convert.FromBase64String(hidden);
                string decodedText = Encoding.UTF8.GetString(decodedBytes);
                return decodedText;
            }
            return null;
        }

        #endregion
        
    }
}
