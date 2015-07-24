using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.SampleApp.Model
{
    [Service]
    public class CustomerService
    {
        private  readonly ICustomerRepository _customerRepository;

        [Inject]
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        public IEnumerable<Customer> FindAll()
        {
            return _customerRepository.FindAll();
        } 
    }
}
