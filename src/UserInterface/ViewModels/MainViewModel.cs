using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
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
            NavigateCommand = new RelayCommand<string>(Navigate, IsNotOnSamePage);
        }

        public void Navigate(string page)
        {
            _navigationService.NavigateTo(page);
        }

        public bool IsNotOnSamePage(string page)
        {
            if (_navigationService.CurrentPageKey != null)
                return !_navigationService.CurrentPageKey.Equals(page);
            return true;
        }
    }
}
