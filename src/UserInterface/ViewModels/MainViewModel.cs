using System;
using System.Net;
using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using Festispec.UI.Views;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Views.Login;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<string> NavigateCommand { get; set; }
        private readonly IFrameNavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        private Account _currentAccount;

        public bool IsLoggedIn => CurrentAccount != null;

        public Account CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value; 
                RaisePropertyChanged(); 
                RaisePropertyChanged(nameof(IsLoggedIn));
                RaisePropertyChanged(nameof(CurrentName));
            }
        }

        public string CurrentName => IsLoggedIn ? CurrentAccount.Employee.Name.First : "Gast";

        public MainViewModel(IFrameNavigationService navigationService, IAuthenticationService authenticationService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            NavigateCommand = new RelayCommand<string>(Navigate,IsNotOnSamePage);
        }
        
        public void Navigate(string page)
        {
            _navigationService.NavigateTo(page);
        }

        public void Login(string username, string password)
        {
            try
            {
                CurrentAccount = _authenticationService.Login(username, password, Role.Employee);
                _navigationService.NavigateTo("HomePage");
            }
            catch (AuthenticationException)
            {
                MessageBox.Show("Incorrect Username or Password.", "Login Failed", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (NotAuthorizedException)
            {
                MessageBox.Show("Not authorized to view this data.", "Role unauthorized", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        public void ShowLoginPageOnStartup()
        {
            _navigationService.NavigateTo("LoginPageEmployee");
        }

        public bool IsNotOnSamePage(string page)
        {
            if (_navigationService.CurrentPageKey != null)
                return !_navigationService.CurrentPageKey.Equals(page);
            return true;
        }
    }
}
