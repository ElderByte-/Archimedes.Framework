using System;
using System.Linq;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.DI
{
    public static class ModuleConfigurationBuilder
    {
        public static IModuleConfiguration BuildAutoConfiguration(params string[] regexFilters)
        {
            var configuration = new ElderModuleConfiguration();
            var registerer = new ComponentRegisterer(configuration);

            var componentScanner = ComponentUtil.BuildComponentScanner();
            var components = componentScanner.ScanByAttribute(regexFilters).ToList();

            registerer.RegisterComponents(components);

            return configuration;
        }
    }
}
