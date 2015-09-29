using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using Archimedes.Framework.Context;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.Localisation.WPF
{
    public class LocalisationExtension : MarkupExtension
    {

        public LocalisationExtension()
        {
            
        }

        public LocalisationExtension(object id)
        {
            Id = id;
        }

        /// <summary>
        /// The text-key for which the translation text should be loaded
        /// </summary>
        [ConstructorArgument("id")]
        public object Id { get; set; }


        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!IsInDesignMode())
            {
                var localisationService = ApplicationContext.Instance.Container.Resolve<ILocalisationService>();
                if (localisationService != null)
                {
                    return localisationService.GetTranslation(Id.ToString());
                }
            }

            return Id;
        }

        private static bool? _isInDesignMode;
        public static bool IsInDesignMode()
        {
            if (!_isInDesignMode.HasValue)
            {
                _isInDesignMode = System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio");
            }
            return _isInDesignMode.Value;
        }
    }
}
