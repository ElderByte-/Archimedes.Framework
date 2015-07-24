using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.SampleApp.Model;

namespace Archimedes.Framework.SampleApp.Mock
{
    public class MemoryCustomerRepository : ICustomerRepository
    {
        private readonly Dictionary<int, Customer> _customers = new Dictionary<int, Customer>(); 


        public IEnumerable<Customer> FindAll()
        {
            return _customers.Values;
        }

        public void Update(Customer customer)
        {
            if (!_customers.ContainsKey(customer.Id))
            {
                _customers.Add(customer.Id, customer);
            }
            else
            {
                _customers[customer.Id] = customer;
            }
        }

        public bool Delete(Customer customer)
        {
            return _customers.Remove(customer.Id);
        }
    }
}
