using System.Collections.Generic;
using Archimedes.Framework.Configuration.Properties;
using Archimedes.Patterns;

namespace Archimedes.Framework.Configuration
{
    /// <summary>
    /// Holds the global configuration of this application.
    /// </summary>
    public interface IEnvironmentService
    {
        /// <summary>
        /// Loads the default configuration using the application.properties files if available.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Gets the current configuration
        /// </summary>
        PropertyStore Configuration { get; }

        /// <summary>
        /// Provides access to the property sources which are configured in this environment.
        /// The order of the property source are very important, in case of conflicts the properties
        /// will be overridden.
        /// 
        /// The last property source in this list has the highest proirity.
        /// </summary>
        List<IPropertySource> PropertySources { get; }
    }
}
