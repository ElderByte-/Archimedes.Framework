using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.SampleApp.Mock;
using Archimedes.Framework.SampleApp.Model;

namespace Archimedes.Framework.SampleApp
{

    [Configuration]
    [ComponentScan("Archimedes.*")]
    public class ApplicationConfiguration
    {

        [ComponentFactory]
        public ICustomerRepository ProvideCustomerMockRepository()
        {
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
