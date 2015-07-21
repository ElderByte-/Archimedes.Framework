using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace Archimedes.Framework.Configuration.Properties
{
    public class FilePropertiesPropertySource : IPropertySource
    {
        private readonly string _filepath;

        public FilePropertiesPropertySource(string filepath)
        {
            _filepath = filepath;
        }

        public PropertyStore Load()
        {
            if (File.Exists(_filepath))
            {
                var rawProperties = File.ReadAllText(_filepath);
                var src = new PropertiesPropertySource(rawProperties);
                return src.Load();
            }

            return new PropertyStore();
        }
    }
}
