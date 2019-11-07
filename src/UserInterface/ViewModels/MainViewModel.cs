using Festispec.DomainServices.Interfaces;
using System.Windows;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel
    {
        public string Name { get => "John Doe";  }

        private readonly IExampleService _exampleService;
        public MainViewModel(IExampleService exampleService)
        {
            _exampleService = exampleService;
            MessageBox.Show(_exampleService.ReturnString());
        }
    }
}
