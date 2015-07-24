using System.Windows;
using System.Windows.Markup.Primitives;
using Archimedes.Framework.Context;
using Archimedes.Framework.ContextEnvironment.Properties;
using Archimedes.Framework.SampleApp.ViewModels;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [Inject]
        private MainViewModel _mainViewModel;

        protected override void OnStartup(StartupEventArgs e)
        {
            var ctx = ApplicationContext.Instance;

            // If necessary, you can add command-line support for the configuration properties
            ctx.Environment.PropertySources.Add(new CommandLinePropertySource(e.Args));

            // EnableAutoConfiguration will do all the heavy lifting and initialize the framework.
            ApplicationContext.Instance.EnableAutoConfiguration();

            // We can now use the IoC Container of Archimedes

            // We need to pull in the root components with a manual call to autowire:
            ApplicationContext.Instance.Container.Autowire(this);


            var main = new MainWindow();
            main.DataContext = _mainViewModel;

            main.Show();

            base.OnStartup(e);
        }

    }
}
