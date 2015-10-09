using System.Reflection;
using System.Windows;
using Archimedes.Framework.Context;
using Archimedes.Framework.ContextEnvironment.Properties;
using Archimedes.Framework.SampleApp.Util;
using Archimedes.Framework.SampleApp.ViewModels;
using Archimedes.Framework.Stereotype;
using log4net;

namespace Archimedes.Framework.SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        [Inject]
        private MainViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Setup logging
            new LoggerConfiguration(AppUtil.ApplicaitonBinaryFolder, "Info");   // log.level will be overridden when archimedes starts

            var context = ApplicationContext.Run();

            // We can now use the IoC Container of Archimedes
            // We need to pull in the root components with a manual call to autowire:
            context.Container.Autowire(this);
            // The _mainViewModel field has now been injected.

            Log.Info("Archimedes Framework initialized, starting main window...");

            var main = new MainWindow
            {
                DataContext = _mainViewModel
            };

            main.Show();

            base.OnStartup(e);
        }

    }
}
