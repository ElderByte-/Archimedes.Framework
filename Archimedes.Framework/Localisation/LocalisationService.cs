using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Archimedes.Framework.ContextEnvironment.Properties;
using Archimedes.Framework.DI.Attribute;
using Archimedes.Framework.Stereotype;
using Archimedes.Patterns.Utils;
using log4net;

namespace Archimedes.Framework.Localisation
{
    /// <summary>
    /// Manages the localisation of the current application
    /// </summary>
    [Service]
    public class LocalisationService : ILocalisationService
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TranslationCache _cache = new TranslationCache();
        private readonly List<IMessageSource> _messageSources = new List<IMessageSource>();

        [Value("${archimedes.culture}")]
        private string _defaultCulture;

        #endregion

        public LocalisationService()
        {
            MessageSources.Add(new FilePropertiesMessageSource(AppUtil.ApplicaitonBinaryFolder + @"\Resources\messages"));

            if (!string.IsNullOrEmpty(_defaultCulture))
            {
                Log.Info(string.Format("Forceing culture '{0}'", _defaultCulture));
                var cultureInfo = new CultureInfo(_defaultCulture);

                // Set the global culture
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
        }

        #region Public API

        public string GetTranslation(string key)
        {
            return GetTranslation(CultureInfo.CurrentUICulture, key);
        }

        public string GetTranslation(string key, params object[] args)
        {
            return GetTranslation(CultureInfo.CurrentUICulture, key, args);
        }

        public string GetTranslation(CultureInfo culture, string key, params object[] args)
        {
            var translation = GetTranslation(culture, key);
            try
            {
                return string.Format(translation, args);
            }
            catch (FormatException e)
            {
                Log.Error(string.Format("Failed to format translation for culture: '{0}' key: '{1}', value: '{2}' with {3} parameters!",
                    culture, key, translation, args.Length)
                    , e);

                return translation;
            }
        }

        public string GetTranslation(CultureInfo culture, string key)
        {
            EnsureCultureLoaded(null);  // Default Culture => null, which is usually english
            EnsureCultureLoaded(culture);

            var translation = _cache.Find(culture, key);

            if (translation != null)
            {
                return translation;
            }

            // Could not find the requested translation, try default fallback
            translation = _cache.Find(null, key);

            if (translation != null)
            {
                Log.Debug(string.Format("Could not find translation for [{0}] in Culture {1}, returning default translation.", key, culture));
                return translation;
            }
            Log.Warn(string.Format("Could not find any translation for key '{0}'!", key));
            return string.Format("[{0}]", key);
        }

        public void Reload()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Gets the list which holds all message sources.
        /// </summary>
        public List<IMessageSource> MessageSources
        {
            get { return _messageSources; }
        }

        #endregion

        #region Private methods

        private void EnsureCultureLoaded(CultureInfo culture)
        {
            if (!_cache.Exists(culture))
            {
                LoadCulture(culture);
            }
        }

        private void LoadCulture(CultureInfo culture)
        {
            Log.Info(string.Format("Loading localisation culture '{0}' ...", culture == null ? "default" : culture.Name));

            var messages = LoadMessageSources(culture);
            _cache.Update(culture, messages.ToKeyValuePairs());
        }

        private PropertyStore LoadMessageSources(CultureInfo culture)
        {
            var ps = new PropertyStore();
            foreach (var messageSource in _messageSources)
            {
                var messages = messageSource.Load(culture);
                ps.Merge(messages);
            }
            return ps;
        }


        #endregion

    }
}
