using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archimedes.DI.AOP;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Configuration;
using Archimedes.Framework.DI;
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
            Environment.Refresh();
            var configuration = Environment.Configuration;

            var assemblyFiltersStr = configuration.GetOptional("archimedes.componentscan.assemblies");
            var assemblyFilters = assemblyFiltersStr.MapOptional(x => x.Split(',')).OrElse(new string[0]);

            var conf = new AutoModuleConfiguration(ScanComponents(assemblyFilters));
            var ctx = RegisterContext(DefaultContext, conf);
            ctx.RegisterInstance<IEnvironmentService>(_environmentService);
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
        /// <param name="configuration"></param>
        public ElderBox RegisterContext(string name, ElderModuleConfiguration configuration)
        {
            var diContext = new ElderBox(configuration);
            _contextRegistry.Add(name, diContext);
            return diContext;
        }

        /// <summary>
        /// Finds all types in the application context which are known components
        /// </summary>
        /// <param name="assemblyFilters">Regexes which allow an assembly if matched.</param>
        /// <returns></returns>
        public IEnumerable<Type> ScanComponents(params string[] assemblyFilters)
        {
            if (_components == null)
            {
                var componentAttributes = new[] {typeof(ServiceAttribute), typeof(ComponentAttribute), typeof(ControllerAttribute)};

                var componentScanner = new AttributeScanner(componentAttributes);
                _components = componentScanner.ScanByAttribute(assemblyFilters).ToList();
            }
            return _components;
        }

        #endregion

    }
}
