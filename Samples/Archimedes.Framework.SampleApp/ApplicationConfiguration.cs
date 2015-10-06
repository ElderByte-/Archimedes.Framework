using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.SampleApp.Mock;
using Archimedes.Framework.SampleApp.Model;

namespace Archimedes.Framework.SampleApp
{

    [Configuration] // Marks this class as a configruation component
    [ComponentScan("Archimedes.*")] // Restrict component-scanning to Archimedes.* assemblies
    [ComponentBinding(typeof(IExternalService), typeof(ExternalService))] // Register a component manually
    public class ApplicationConfiguration
    {

        [ComponentFactory]
        public ICustomerRepository ProvideCustomerMockRepository(IDummyConfigurationService dependency)
        {
            var value = dependency.GetProperty("blub");

            var repo = new MemoryCustomerRepository();

            repo.Update(new Customer(1)
            {
                Name = "John Doe",
                Age = 30
            });

            repo.Update(new Customer(2)
            {
                Name = "Max Doe",
                Age = 12
            });

            repo.Update(new Customer(3)
            {
                Name = "Martin Cruz",
                Age = 45
            });

            return repo;
        }

    }
}
