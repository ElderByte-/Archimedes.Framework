using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.Configuration.Properties
{
    public interface IPropertySource
    {
        /// <summary>
        /// Load the properties from this source
        /// </summary>
        /// <returns></returns>
        PropertyStore Load();
    }
}
