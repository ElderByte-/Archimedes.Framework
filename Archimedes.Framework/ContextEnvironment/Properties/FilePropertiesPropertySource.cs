using System.IO;

namespace Archimedes.Framework.ContextEnvironment.Properties
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
