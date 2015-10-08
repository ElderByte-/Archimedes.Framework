using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archimedes.Framework.Context.Configuration;
using Archimedes.Framework.ContextEnvironment;
using Archimedes.Framework.DI;
using Archimedes.Framework.Stereotype;
using log4net;

namespace Archimedes.Framework.Context
{
    /// <summary>
    /// Represents the Application Context, which provides component scanning / auto-configuration.
    /// </summary>
    public class ApplicationContext
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IEnvironmentService _environmentService = new EnvironmentService();

        private readonly ElderBox _container;
        private List<Type> _components = null; // Lazy initialized!

        #endregion

        #region Static builder methods


        public static ApplicationContext Run()
        {
            var context = new ApplicationContext();
            return context;
        }


        #endregion

        #region Constructor

        private ApplicationContext()
        {
            _container = new ElderBox();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the environment which holds the basic configuration of the application
        /// </summary>
        public IEnvironmentService Environment
        {
            get { return _environmentService; }
        }

        /// <summary>
        /// Gets the DI container 
        /// </summary>
        /// <returns></returns>
        public ElderBox Container
        {
            get { return _container; }
        }


        #endregion

        #region Public methods


        /// <summary>
        ///  Enables Auto-Configuration, which basically scans for Components, handles the configuration
        /// and makes the app ready for usage.
        /// 
        ///  Components must be marked with [Service] or [Controller].
        /// </summary>
        public void EnableAutoConfiguration()
        {
            try
            {
                var configuration = Environment.Configuration;

                var configurationLoader = new ConfigurationLoader(this);
                configurationLoader.Load();

                var assemblyFiltersStr = configuration.GetOptional(ArchimedesPropertyKeys.ComponentScanAssemblies);
                var assemblyFilters = assemblyFiltersStr.Map(x => x.Split(',')).OrElse(new string[0]);

                var configurer = new ComponentRegisterer(_container.Configuration);
                configurer.RegisterComponents(ScanComponents(assemblyFilters));

                _container.RegisterInstance<IEnvironmentService>(_environmentService);
                _container.RegisterInstance<ApplicationContext>(this);
            }
            catch (Exception e)
            {
                throw;
            }
        }

       
        /// <summary>
        /// Finds all types in the application context which are known components
        /// </summary>
        /// <param name="assemblyFilters">Regexes which allow an assembly if matched.</param>
        /// <returns></returns>
        private IEnumerable<Type> ScanComponents(params string[] assemblyFilters)
        {
            if (_components == null)
            {
                var componentScanner = ComponentUtil.BuildComponentScanner();
                Log.Info("Scanning for [Component] / [Service] / etc. classes...");
                _components = componentScanner.ScanByAttribute(assemblyFilters).ToList();
            }
            return _components;
        }

        #endregion

    }
}
