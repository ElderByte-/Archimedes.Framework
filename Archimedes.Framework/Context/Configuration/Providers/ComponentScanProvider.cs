using System;
using System.Linq;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.ContextEnvironment;

namespace Archimedes.Framework.Context.Configuration.Providers
{
    internal class ComponentScanProvider : IConfigurationProvider
    {
        public void HandleConfiguration(ApplicationContext ctx, Type configurationType)
        {
            var componentScans = configurationType.GetCustomAttributes(typeof(ComponentScanAttribute), false);
            if (componentScans.Length > 0)
            {
               var  componentScan = componentScans.First() as ComponentScanAttribute;
               var filterRegex = componentScan.IncludeFilterRegex;

                // TODO this gets lost when refresh is called
               ctx.Environment.Configuration.Set(ArchimedesPropertyKeys.ComponentScanAssemblies, filterRegex);
            }
        }
    }
}

