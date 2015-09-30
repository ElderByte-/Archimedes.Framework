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

            foreach (var file in GetAvailablePropertiesFiles())
            {
                if (IsMatch(culture, file.Name))
                {
                    var propertiesSource = new FilePropertiesPropertySource(file.FullName);
                    allmessages.Merge(propertiesSource.Load());
                }
            }
            
            return allmessages;
        }


        private IEnumerable<FileInfo> GetAvailablePropertiesFiles()
        {
            if (_messagesFolder.Exists)
            {
                foreach (var file in _messagesFolder.GetFiles())
                {
                    if (file.Extension == ".properties")
                    {
                        yield return file;
                    }
                }
            }
        } 

        public ISet<CultureInfo> GetAvailableCultures()
        {
            var availables = new HashSet<CultureInfo>();

            foreach (var file in GetAvailablePropertiesFiles())
            {
                availables.Add(CultureInfoByName(file.Name));
            }

            return availables;
        }

        public static bool IsMatch(CultureInfo culture, string fileName)
        {
            var fileCulture = CultureInfoByName(fileName);
            return culture.Equals(fileCulture);
        }

        private static CultureInfo CultureInfoByName(string fileName)
        {
            fileName = Path.GetFileNameWithoutExtension(fileName);

            var parts = fileName.Split('_');

            string isoCode = null;
            string cultureCode = null;

            if (parts.Length == 2)
            {
                isoCode = parts[1];
            }
            else if (parts.Length > 2)
            {
                isoCode = parts[1];
                cultureCode = parts[2];
            }

            if (isoCode == null) return CultureInfo.InvariantCulture;
            if (cultureCode == null) return CultureInfo.GetCultureInfo(isoCode);
            return CultureInfo.GetCultureInfo(isoCode + "-" + cultureCode.ToUpper());
        }


    }
}
