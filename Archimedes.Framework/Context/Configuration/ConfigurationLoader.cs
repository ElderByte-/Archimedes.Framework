using System;
using System.Collections.Generic;
using System.Reflection;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.Context.Configuration.Providers;
using log4net;

namespace Archimedes.Framework.Context.Configuration
{
    public class ConfigurationLoader
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly List<IConfigurationProvider> _configurationProcessors = new List<IConfigurationProvider>();

        private readonly ApplicationContext _ctx;

        #endregion

        /// <summary>
        /// Creates a new configuration loader for the given application context.
        /// </summary>
        /// <param name="ctx">The application context</param>
        public ConfigurationLoader(ApplicationContext ctx)
        {
            _ctx = ctx;
            ConfigurationProcessors.Add(new ComponentScanProvider());
            ConfigurationProcessors.Add(new ComponentFactoryProvider());
        }

        #region Public Properties

        /// <summary>
        /// Holds the registered configuration processors.
        /// </summary>
        public List<IConfigurationProvider> ConfigurationProcessors
        {
            get { return _configurationProcessors; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Loads the configuration
        /// </summary>
        public void Load()
        {
            Log.Info("Loading configuration classes...");

            var configurationTypes = FindAllConfigurationTypes();

            foreach (var configurationType in configurationTypes)
            {
                LoadConfiguration(configurationType);
            }
        }

        #endregion

        #region Private methods

        private void LoadConfiguration(Type configurationType)
        {
            foreach (var configurationProcessor in ConfigurationProcessors)
            {
                Log.Info(string.Format("Applying configuration class '{0}'", configurationType));
                configurationProcessor.HandleConfiguration(_ctx, configurationType);
            }
        }


        private ISet<Type> FindAllConfigurationTypes()
        {
            var configurationScanner = new AttributeScanner(typeof(ConfigurationAttribute));

            var assembly = Assembly.GetEntryAssembly();
            string assemblyFilter = null;
            if (assembly != null)
            {
                assemblyFilter = assembly.GetName().Name;
            }

            var configurationTypes = configurationScanner.ScanByAttribute(assemblyFilter);

            // Use component-scan filters to recursilvy scan for more configuration types in the assembly.

            return configurationTypes;
        }

        #endregion

    }
}
