using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Archimedes.Framework.Context.Annotation;
using log4net;

namespace Archimedes.Framework.SampleApp
{
    [Configuration] // Configurations are always eagerly created ...
    public class SampleConfiguration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SampleConfiguration()
        {
            Log.Info("SampleConfiguration created!");   // ... thus its constructor is automatically invoked after the context is configured
        }
    }
}
