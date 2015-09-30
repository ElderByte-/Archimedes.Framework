using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using Archimedes.Framework.Context;
using Archimedes.Framework.Stereotype;
using log4net;

namespace Archimedes.Framework.Localisation.WPF
{
    public class LocalisationExtension : MarkupExtension
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


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
            try
            {
                if (!IsInDesignMode())
                {
                    var diContainer = ApplicationContext.Instance.Container;
                    if (diContainer != null)
                    {
                        var localisationService = diContainer.Resolve<ILocalisationService>();
                        return localisationService.GetTranslation(Id.ToString());
                    }
                    else
                    {
                        throw new NotSupportedException("Archimedes Dependency Injection Container is not ready, can not access ILocalisationService!");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Failed to provide localized text!", e);
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
