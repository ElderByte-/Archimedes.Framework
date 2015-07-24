using System;
using System.Linq;
using System.Reflection;
using Archimedes.Framework.Context.Annotation;
using log4net;

namespace Archimedes.Framework.Context.Configuration.Providers
{
    internal class ComponentBindingProvider : IConfigurationProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public void HandleConfiguration(ApplicationContext ctx, Type configurationType)
        {
            var componentScans = configurationType.GetCustomAttributes(typeof(ComponentBindingAttribute), false);
            if (componentScans.Length > 0)
            {
                var componentBinding = componentScans.First() as ComponentBindingAttribute;
                ctx.Container.Configuration.RegisterSingleton(componentBinding.Iface, componentBinding.Implementation);
                Log.Info(string.Format("Configured component binding '{0}' --> '{1}'", componentBinding.Iface, componentBinding.Implementation));
            }
        }
    }
}
