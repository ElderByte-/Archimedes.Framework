using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Archimedes.Framework.Configuration.Properties;

namespace Archimedes.Framework.SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

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
            main.Show();

            base.OnStartup(e);
        }

    }
}
