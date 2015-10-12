using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.Context.Configuration;
using Archimedes.Framework.ContextEnvironment;
using Archimedes.Framework.DI;
using Archimedes.Framework.Stereotype;
using log4net;
using log4net.Repository.Hierarchy;

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

        private bool _isAutoConfigured = false;

        #endregion

        #region Events

        /// <summary>
        /// Fired when this application context is initialized
        /// </summary>
        public event EventHandler ContextInitialized;

        #endregion

        #region Static builder methods

        /// <summary>
        /// Runs the application and creates a applicaiton context
        /// </summary>
        /// <returns></returns>
        public static ApplicationContext Run()
        {
            var context = new ApplicationContext();
            context.EnableAutoConfiguration();
            return context;
        }

        #endregion

        #region Constructor

        protected ApplicationContext()
        {
            _container = new ElderBox();

            var textArt = string.Format(@"
                    _     _                    _           
     /\            | |   (_)                  | |          
    /  \   _ __ ___| |__  _ _ __ ___   ___  __| | ___  ___ 
   / /\ \ | '__/ __| '_ \| | '_ ` _ \ / _ \/ _` |/ _ \/ __|
  / ____ \| | | (__| | | | | | | | | |  __/ (_| |  __/\__ \
 /_/    \_\_|  \___|_| |_|_|_| |_| |_|\___|\__,_|\___||___/
            v{0}                                               
                                                           ", typeof(ApplicationContext).Assembly.GetName().Version);

            Log.Info("Starting Archimedes.Framework ...\n" + textArt);

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

        #region Private methods

        /// <summary>
        ///  Enables Auto-Configuration, which basically scans for Components, handles the configuration
        /// and makes the app ready for usage.
        /// 
        ///  Components must be marked with [Service] or [Controller].
        /// </summary>
        protected void EnableAutoConfiguration()
        {
            if (!_isAutoConfigured)
            {
                try
                {
                    var configuration = Environment.Configuration;

                    configuration.GetOptional("application.log.level").IfPresent(logLevel =>
                    {
                        var hierarchy = (Hierarchy) LogManager.GetRepository();
                        hierarchy.Root.Level = hierarchy.LevelMap[logLevel];
                    });

                    var configurationLoader = new ConfigurationLoader(this);
                    configurationLoader.Load();

                    var assemblyFiltersStr = configuration.GetOptional(ArchimedesPropertyKeys.ComponentScanAssemblies);
                    var assemblyFilters = assemblyFiltersStr.Map(x => x.Split(',')).OrElse(new string[0]);

                    var configurer = new ComponentRegisterer(_container.Configuration);
                    var allFoundComponents = ScanComponents(assemblyFilters);
                    configurer.RegisterComponents(allFoundComponents);

                    _container.RegisterInstance<IEnvironmentService>(_environmentService);
                    _container.RegisterInstance<ApplicationContext>(this);

                    _isAutoConfigured = true;

                    OnContextInitialized(allFoundComponents);
                }
                catch (Exception e)
                {
                    throw new ApplicationContextInitialisationException("Failed to initialize and configure application context!", e);
                }
            }
        }

        /// <summary>
        /// Finds all types in the application context which are known components
        /// </summary>
        /// <param name="assemblyFilters">Regexes which allow an assembly if matched.</param>
        /// <returns></returns>
        private IEnumerable<Type> ScanComponents(params string[] assemblyFilters)
        {
            var componentScanner = ComponentUtil.BuildComponentScanner();
            Log.Info("Scanning for [Component] / [Service] / etc. classes...");
            return componentScanner.ScanByAttribute(assemblyFilters).ToList();
        }

        /// <summary>
        /// Instantiate eager components
        /// </summary>
        private void InstantiateEagerComponents(IEnumerable<Type> components)
        {

            foreach (var componentType in components)
            {
                if (IsEagerRequested(componentType))
                {
                    try
                    {
                        _container.Resolve(componentType);
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationContextException(string.Format("Failed to eager load component '{0}'!", componentType), e);
                    }
                }
            }
        }

        private bool IsEagerRequested(Type componentType)
        {
            // Components makred with [Eager]
            if (Attribute.IsDefined(componentType, typeof(EagerAttribute)))
            {
                return true;
            }

            // Configuration Components are always loaded eagerly
            if (Attribute.IsDefined(componentType, typeof(ConfigurationAttribute)))
            {
                return true;
            }

            // Components which have any method marked with [PostConstruct]
            var hasPostConstruct = componentType.GetMethods().Any(x => System.Attribute.IsDefined(x, typeof(PostConstructAttribute)));
            if (hasPostConstruct)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Occurs when this application context has loaded
        /// </summary>
        private void OnContextInitialized(IEnumerable<Type> components)
        {
            if (ContextInitialized != null)
                ContextInitialized(this, EventArgs.Empty);

            InstantiateEagerComponents(components);
            
        }

        #endregion

    }
}
