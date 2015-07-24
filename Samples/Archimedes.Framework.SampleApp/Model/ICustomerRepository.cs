using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.SampleApp.Model
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> FindAll();

        void Update(Customer customer);

        bool Delete(Customer customer);
    }
}
