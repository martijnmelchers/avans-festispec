using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
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
            Navigate("Homepage");
            NavigateCommand = new RelayCommand<string>(Navigate, IsOnSamePage);

            foreach(string page in _navigationService.Pages)
            {
                MessageBox.Show(page);

            }
        }

        public void Navigate(string page)
        {
            MessageBox.Show(page);
            _navigationService.NavigateTo(page);
        }

        public bool IsOnSamePage(string page)
        {
            if (_navigationService.CurrentPageKey != null)
                return !_navigationService.CurrentPageKey.Equals(page);
            return false;
        }
    }
}
