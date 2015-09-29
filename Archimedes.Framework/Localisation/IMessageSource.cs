using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Archimedes.Framework.ContextEnvironment.Properties;

namespace Archimedes.Framework.Localisation
{
    public interface IMessageSource
    {
        PropertyStore Load(CultureInfo culture);
    }
}
