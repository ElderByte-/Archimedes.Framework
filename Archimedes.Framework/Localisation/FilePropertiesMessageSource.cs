using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Archimedes.Framework.ContextEnvironment.Properties;
using log4net;

namespace Archimedes.Framework.Localisation
{
    public class FilePropertiesMessageSource : IMessageSource
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DirectoryInfo _messagesFolder;

        public FilePropertiesMessageSource(string messagesFolder)
        {
            if(messagesFolder == null) throw new ArgumentNullException("messagesFolder");

            _messagesFolder = new DirectoryInfo(messagesFolder);
        }

        /// <summary>
        /// By default, all *.properties files are loaded which match the given culture.
        /// 
        /// myResource.properties
        /// myResource_de.properties
        /// myResource_de_CH.properties
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public PropertyStore Load(CultureInfo culture)
        {
            var allmessages = new PropertyStore();

            Log.Info(string.Format("Loading culture {0} -> Scanning for localized messages in folder '{1}'...", culture, _messagesFolder.FullName));

            if (_messagesFolder.Exists)
            {
                foreach (var file in _messagesFolder.GetFiles())
                {
                    if (file.Extension == ".properties")
                    {
                        if (IsMatch(culture, file.Name))
                        {
                            var propertiesSource = new FilePropertiesPropertySource(file.FullName);
                            allmessages.Merge(propertiesSource.Load());
                        }
                    }
                }
            }
            return allmessages;
        }

        public static bool IsMatch(CultureInfo culture, string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);
            var parts = fileName.Split('_');

            string isoCode = null;
            string cultureCode = null;

            if (parts.Length == 2)
            {
                isoCode = parts[1];
            }else if (parts.Length > 2)
            {
                isoCode = parts[1];
                cultureCode = parts[2];
            }

            if (culture == null)
            {
                return isoCode == null;
            }

            if (isoCode != null)
            {
                var expectedParts = culture.Name.Split('-');
                var expectedIsoCode = expectedParts[0];
                var expectedCulture = expectedParts.Length > 1 ? expectedParts[1] : null;

                if (string.Equals(isoCode, expectedIsoCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (cultureCode == null) return true;

                    return expectedCulture == null || string.Equals(cultureCode, expectedCulture, StringComparison.CurrentCultureIgnoreCase);
                }
            }

            return false;
        }


    }
}
