using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.SampleApp.ViewModels
{
    [Controller]
    public class MainViewModel
    {
        [Inject]
        private CustomersViewModel _customersViewModel;

        public string Title
        {
            get { return "Hello Archimedes!"; }
        }


        public CustomersViewModel CustomersVm
        {
            get
            {
                return _customersViewModel;
            }
        }

    }
}
