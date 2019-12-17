using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Festispec.UI.Views;
using System.Windows;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<string> NavigateCommand { get; set; }
        private readonly IFrameNavigationService _navigationService;

        public MainViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand<string>(Navigate);
        }

        public void Navigate(string page)
        {
            _navigationService.NavigateTo(page);
        }
    }
}
