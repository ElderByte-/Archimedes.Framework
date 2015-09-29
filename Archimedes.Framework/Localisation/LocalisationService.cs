using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Archimedes.Framework.ContextEnvironment.Properties;
using Archimedes.Patterns.Utils;

namespace Archimedes.Framework.Localisation
{
    public class LocalisationService : ILocalisationService
    {
        #region Fields

        private readonly TranslationCache _cache = new TranslationCache();
        private readonly List<IMessageSource> _messageSources = new List<IMessageSource>(); 

        #endregion

        public LocalisationService()
        {
            MessageSources.Add(new FilePropertiesMessageSource(AppUtil.ApplicaitonBinaryFolder + @"\i18"));
        }

        #region Public API

        public string GetTranslation(string key)
        {
            return GetTranslation(CultureInfo.CurrentUICulture, key);
        }

        public string GetTranslation(string key, params object[] args)
        {
            return string.Format(GetTranslation(key), args);
        }

        public string GetTranslation(CultureInfo culture, string key)
        {
            EnsureCultureLoaded(culture);

            var translation = _cache.Find(culture, key);

            if (translation != null)
            {
                return translation;
            }
            // TODO Fall back to default language?

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
