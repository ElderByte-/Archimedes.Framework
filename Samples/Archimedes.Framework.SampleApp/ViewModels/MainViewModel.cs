using Archimedes.Framework.DI.Attribute;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.SampleApp.ViewModels
{
    [Controller]
    public class MainViewModel
    {

        [Value("${application.title}")] 
        private string _title;

        [Inject]
        private CustomersViewModel _customersViewModel;

        public string Title
        {
            get { return _title; }
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
