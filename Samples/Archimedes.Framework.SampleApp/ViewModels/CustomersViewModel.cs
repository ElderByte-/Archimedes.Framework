using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Archimedes.Framework.Localisation;
using Archimedes.Framework.SampleApp.Model;
using Archimedes.Framework.SampleApp.Util;
using Archimedes.Framework.Stereotype;
using Archimedes.Localisation;

namespace Archimedes.Framework.SampleApp.ViewModels
{
    [Controller]
    public class CustomersViewModel
    {
        private readonly CustomerService _customerService;
        private readonly ILocalisationService _localisationService;
        private Customer _currentCustomer;

        [Inject]
        public CustomersViewModel(CustomerService customerService, ILocalisationService localisationService)
        {
            _customerService = customerService;
            _localisationService = localisationService;
        }

        public IEnumerable<Customer> AllCustomers
        {
            get { return _customerService.FindAll(); }
        }

        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set
            {
                _currentCustomer = value;
            }
        }


        public ICommand ShowDetailCommand
        {
            get {
                return new RelayCommand(x =>
                {
                    MessageBox.Show(_localisationService.GetTranslation("customers.detail.message", CurrentCustomer));
                }
            ); }
        }
    }
}
