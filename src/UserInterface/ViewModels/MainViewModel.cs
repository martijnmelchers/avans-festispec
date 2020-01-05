﻿using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Festispec.DomainServices.Services;

namespace Festispec.UI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand<string> NavigateCommand { get; set; }
        private readonly IFrameNavigationService _navigationService;
        private readonly IAuthenticationService _authenticationService;
        private Account _currentAccount;

        public ICommand LoginCommand { get; set; }
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
                RaisePropertyChanged(nameof(HideNavbar));
            }
        }

        public string CurrentUsername { get; set; }
        public string CurrentName => IsLoggedIn ? CurrentAccount.Employee.Name.First : "Gast";

        public Visibility HideNavbar => !IsLoggedIn ? Visibility.Hidden : Visibility.Visible; //navbar visible or hidden.
        
        public Visibility IsOffline { get; set; }

        public MainViewModel(IFrameNavigationService navigationService, IAuthenticationService authenticationService, OfflineService offlineService)
        {
            _navigationService = navigationService;
            _authenticationService = authenticationService;
            IsOffline = offlineService.IsOnline ? Visibility.Hidden : Visibility.Visible;
            NavigateCommand = new RelayCommand<string>(Navigate, IsNotOnSamePage);
            LoginCommand = new RelayCommand<object>(Login);
        }

        public void Navigate(string page)
        {
            _navigationService.NavigateTo(page);
        }

        public void Login(object passwordBox)
        {
            try
            {
                CurrentAccount = _authenticationService.Login(CurrentUsername, ((PasswordBox)passwordBox).Password, Role.Employee);
                _authenticationService.Sync();
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