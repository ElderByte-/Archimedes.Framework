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
    public sealed class ApplicationContext
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string DefaultContext = "_ELDER_DEFAULT";
        private readonly Dictionary<string, ElderBox> _contextRegistry = new Dictionary<string, ElderBox>();
        private readonly IEnvironmentService _environmentService = new EnvironmentService();

        private List<Type> _components = null; // Lazy initialized!

        #endregion

        #region Singleton

        private static readonly ApplicationContext _instance = new ApplicationContext();

        public static ApplicationContext Instance
        {
            get { return _instance; }
        }

        private ApplicationContext()
        {
            
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
        /// Gets the default context which is populated by the auto configuration.
        /// </summary>
        /// <returns></returns>
        public ElderBox Container
        {
            get { return _contextRegistry[DefaultContext]; }
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
                var container = new ElderBox();
                var ctx = RegisterContext(DefaultContext, container);

                var configuration = Environment.Configuration;

                var configurationLoader = new ConfigurationLoader(this);
                configurationLoader.Load();

                var assemblyFiltersStr = configuration.GetOptional(ArchimedesPropertyKeys.ComponentScanAssemblies);
                var assemblyFilters = assemblyFiltersStr.Map(x => x.Split(',')).OrElse(new string[0]);

                var configurer = new ComponentRegisterer(container.Configuration);
                configurer.RegisterComponents(ScanComponents(assemblyFilters));

                ctx.RegisterInstance<IEnvironmentService>(_environmentService);
                ctx.RegisterInstance<ApplicationContext>(this);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        

        /// <summary>
        /// Get a named DI context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ElderBox ContainerByName(string name)
        {
            if (_contextRegistry.ContainsKey(name))
            {
                return _contextRegistry[name];
            }
            
            throw new NotSupportedException("ElderBox Context with name '" + name + "' could not be found. Did you forget to register it?" );
        }

        /// <summary>
        /// Register a named DI context.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="diContext"></param>
        public ElderBox RegisterContext(string name, ElderBox diContext)
        {
            _contextRegistry.Add(name, diContext);
            return diContext;
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
