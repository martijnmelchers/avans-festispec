using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel : BaseValidationViewModel
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IFrameNavigationService _navigationService;
        private Account _currentAccount;

        public MainViewModel(IFrameNavigationService navigationService, IAuthenticationService authenticationService, IOfflineService offlineService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            IsOffline = offlineService.IsOnline ? Visibility.Hidden : Visibility.Visible;
            NavigateCommand = new RelayCommand<string>(NavigateToPage, IsNotOnSamePage, true);
            LoginCommand = new RelayCommand<object>(Login);
        }

        public RelayCommand<string> NavigateCommand { get; set; }

        public ICommand LoginCommand { get; set; }
        public bool IsLoggedIn => CurrentAccount != null;

        private Account CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsLoggedIn));
                RaisePropertyChanged(nameof(CurrentName));
                RaisePropertyChanged(nameof(HideNavbar));
            }
        }

        public string CurrentUsername { get; set; }
        public string CurrentName => IsLoggedIn ? CurrentAccount.Employee.Name.First : "Gast";

        public Visibility HideNavbar => !IsLoggedIn ? Visibility.Hidden : Visibility.Visible; //navbar visible or hidden.
        
        public Visibility IsOffline { get; set; }
        
        private void NavigateToPage(string page)
        {
            _navigationService.NavigateTo(page);
            NavigateCommand.RaiseCanExecuteChanged();
        }

        private void Login(object passwordBox)
        {
            try
            {
                CurrentAccount = _authenticationService.Login(CurrentUsername, ((PasswordBox)passwordBox).Password, Role.Employee);
                _authenticationService.Sync();
                _navigationService.NavigateTo("HomePage");
            }
            catch (AuthenticationException)
            {
                OpenValidationPopup("Incorrecte gebruikersnaam of wachtwoord.");
            }
            catch (NotAuthorizedException)
            {
                OpenValidationPopup("Niet toegestaan deze data in te zien.");
            }
        }

        public void ShowLoginPageOnStartup()
        {
            _navigationService.NavigateTo("LoginPageEmployee");
        }

        private bool IsNotOnSamePage(string page)
        {
            return _navigationService.CurrentPageKey == null || !_navigationService.CurrentPageKey.Equals(page);
        }
    }
}