using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.Localisation
{
    public interface ILocalisationService
    {
        string GetTranslation(string key);

        string GetTranslation(string key, params object[] args);


        string GetTranslation(CultureInfo culture, string key);

        void Reload();

    }
}
