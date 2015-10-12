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
using Archimedes.Localisation;
using Archimedes.Patterns.Utils;
using log4net;

namespace Archimedes.Framework.Localisation
{
    /// <summary>
    /// Manages the localisation of the current application
    /// </summary>
    [Eager]
    [Service]
    public class LocalisationServiceImpl : LocalisationService
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Value("${archimedes.culture}")]
        private string _defaultCulture;

        #endregion

        public LocalisationServiceImpl()
        {
            Log.Info("Initializing Localisation...");

            // Ensure the Translator shares our location service (avoids loading a language twice)
            Translator.TranslationProvider = this;  

            // Support for forced culture settings out of the box
            if (!string.IsNullOrEmpty(_defaultCulture))
            {
                Log.Info(string.Format("Forceing culture '{0}'", _defaultCulture));
                var cultureInfo = new CultureInfo(_defaultCulture);

                // Set the global culture
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            }
        }
    }
}
