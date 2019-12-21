using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Exceptions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class AccountViewModel : BaseValidationViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IFrameNavigationService _navigationService;

        public AccountViewModel(IFrameNavigationService navigationService, IEmployeeService employeeService)
        {
            _navigationService = navigationService;
            _employeeService = employeeService;
            
            if (navigationService.Parameter == null || !(navigationService.Parameter is int employeeId))
                throw new InvalidNavigationException();

            Account = employeeService.GetAccountForEmployee(employeeId);
            NavigateBackCommand = new RelayCommand(NavigateBack);
            SaveCommand = new RelayCommand<PasswordWithVerification>(SaveChanges);
        }

        public Account Account { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand SaveCommand { get; }

        private void NavigateBack()
        {
            _navigationService.NavigateTo("UpdateEmployee", Account.Id);
        }

        public void SaveChanges(PasswordWithVerification passwordWithVerification)
        {
            if (!passwordWithVerification.BothEmpty())
            {
                if (!passwordWithVerification.Equal() || passwordWithVerification.Empty())
                {
                    ValidationError = "Er is geen wachtwoord ingevuld of de wachtwoorden komen niet overeen.";
                    PopupIsOpen = true;
                    return;
                }

                if (passwordWithVerification.Password.Length < 5)
                {
                    ValidationError = "Het wachtwoord moet tussen 5 en 100 karakters zijn.";
                    PopupIsOpen = true;
                    return;
                }

                IntPtr valuePtr = IntPtr.Zero;
                try
                {
                    valuePtr = Marshal.SecureStringToGlobalAllocUnicode(passwordWithVerification.Password);
                    Account.Password = BCrypt.Net.BCrypt.HashPassword(Marshal.PtrToStringUni(valuePtr));
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                    passwordWithVerification.Dispose();
                }
            }
            
            if (!Account.Validate())
            {
                ValidationError = "De ingevoerde data klopt niet of is involledig.";
                PopupIsOpen = true;
                return;
            }

            _employeeService.SaveChangesAsync();
            NavigateBack();
        }
    }
}