using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.Localisation
{
    /// <summary>
    /// Provides localisation services such as translations for different cultures.
    /// </summary>
    public interface ILocalisationService
    {

        /// <summary>
        /// Manages all registered message-sources
        /// </summary>
        List<IMessageSource> MessageSources { get; }
    


        /// <summary>
        /// Get the translation in the current UI Culture for the given text-key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTranslation(string key);

        /// <summary>
        /// Get the translation in the current UI Culture for the given text-key,
        /// formating the string using the provided arguments.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetTranslation(string key, params object[] args);

        /// <summary>
        /// Get the translation for the given culture and text-key
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetTranslation(CultureInfo culture, string key);

        /// <summary>
        /// Get the translation in the given culture for the given text-key,
        /// formating the string using the provided arguments.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        string GetTranslation(CultureInfo culture, string key, params object[] args);

        /// <summary>
        /// Ensures that all translations are updated from the currently registered MessageSources
        /// </summary>
        void Reload();

    }
}
