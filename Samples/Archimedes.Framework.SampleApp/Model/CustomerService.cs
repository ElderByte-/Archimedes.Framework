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
        private readonly IExternalService _externalService;

        [Inject]
        public CustomerService(ICustomerRepository customerRepository, IExternalService externalService)
        {
            _customerRepository = customerRepository;
            _externalService = externalService;
        }


        public IEnumerable<Customer> FindAll()
        {
            return _customerRepository.FindAll();
        } 
    }
}
