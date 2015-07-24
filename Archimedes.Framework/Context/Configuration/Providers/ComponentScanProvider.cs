using System;
using System.Linq;
using System.Reflection;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.ContextEnvironment;
using log4net;

namespace Archimedes.Framework.Context.Configuration.Providers
{
    internal class ComponentScanProvider : IConfigurationProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public void HandleConfiguration(ApplicationContext ctx, Type configurationType)
        {
            var componentScans = configurationType.GetCustomAttributes(typeof(ComponentScanAttribute), false);
            if (componentScans.Length > 0)
            {
               var  componentScan = componentScans.First() as ComponentScanAttribute;
               var filterRegex = componentScan.IncludeFilterRegex;

                // TODO this gets lost when refresh is called
               ctx.Environment.Configuration.Set(ArchimedesPropertyKeys.ComponentScanAssemblies, filterRegex);

               Log.Info(string.Format("Configured Component-Scan Filter as '{0}'", filterRegex));
            }
        }
    }
}

