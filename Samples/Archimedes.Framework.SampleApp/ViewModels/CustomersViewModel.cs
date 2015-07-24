using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Archimedes.Framework.SampleApp.Model;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.SampleApp.ViewModels
{
    [Controller]
    public class CustomersViewModel
    {
        private readonly CustomerService _customerService;

        [Inject]
        public CustomersViewModel(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public IEnumerable<Customer> AllCustomers
        {
            get { return _customerService.FindAll(); }
        } 

    }
}
