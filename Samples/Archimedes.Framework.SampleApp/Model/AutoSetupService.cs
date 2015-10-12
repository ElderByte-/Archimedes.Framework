using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.DI.Attribute;
using Archimedes.Framework.Stereotype;
using log4net;

namespace Archimedes.Framework.SampleApp.Model
{
    /// <summary>
    /// Example of a service which is Eagerly initialized and has a PostConstruct handler
    /// </summary>
    [Service]
    public class AutoSetupService
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        [Value("${application.autoSetup.property1}")]
        private string autoSetupProperty1;

        [Value("${application.autoSetup.property2}")]
        private bool autoSetupProperty2;

        /// <summary>
        /// This method is called after the app context is initialized and this component has been created
        /// </summary>
        [PostConstruct]
        public void Init()
        {
            Log.Info(string.Format("The AutoSetupService is no configuring, using property '{0}' and '{1}'!", autoSetupProperty1, autoSetupProperty2));
        }

    }
}
