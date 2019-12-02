using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Windows;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Name { get => "John Doe";  }
        public RelayCommand<string> NavigateCommand { get; set; }
        private readonly IFrameNavigationService _navigationService;
        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand<string>(Navigate);
        }

        public void Navigate(string page)
        {
            MessageBox.Show(page);
            _navigationService.NavigateTo(page);
        }
    }
}
