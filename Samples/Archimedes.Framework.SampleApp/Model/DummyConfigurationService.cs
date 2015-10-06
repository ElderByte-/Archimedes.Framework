using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.SampleApp.Model
{
    [Service]
    public class DummyConfigurationService : IDummyConfigurationService
    {
        public string GetProperty(string key)
        {
            return "[" + key  + "]";

        }
    }
}
