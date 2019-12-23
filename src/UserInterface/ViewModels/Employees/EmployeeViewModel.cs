using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class EmployeeViewModel : BaseValidationViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IFrameNavigationService _navigationService;

        public EmployeeViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService)
        {
            _employeeService = employeeService;
            _navigationService = navigationService;
            ViewCertificatesCommand = new RelayCommand(ViewCertificates);

            if (_navigationService.Parameter is int customerId)
            {
                Employee = _employeeService.GetEmployee(customerId);
                CanDeleteEmployee = _employeeService.CanRemoveEmployee(Employee);
                SaveCommand = new RelayCommand(UpdateEmployee);
                Caption = "Medewerker Toevoegen";
            }
            else
            {
                Employee = new Employee {Account = new Account()};
                SaveCommand = new RelayCommand<PasswordWithVerification>(AddEmployee);
                Caption = "Medewerker Wijzigen";
            }

            CancelCommand = new RelayCommand(NavigateToAccount);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployee);
            EditEmployeeCommand = new RelayCommand(NavigateToEditEmployee);
            EditAccountCommand = new RelayCommand(NavigateToEditAccount);
            NavigateBackCommand = new RelayCommand(NavigateBack);
        }

        public Employee Employee { get; }

        public ICommand SaveCommand { get; }
        public ICommand RemoveEmployeeCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand EditEmployeeCommand { get; }

        public ICommand EditAccountCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public bool CanDeleteEmployee { get; }

        public ICommand ViewCertificatesCommand { get; }

        private void ViewCertificates()
        {
            _navigationService.NavigateTo("CertificateList", Employee.Id);
        }

        private void NavigateToAccount()
        {
            _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
        }

        private void NavigateToEditAccount()
        {
            _navigationService.NavigateTo("UpdateAccount", Employee.Id);
        }

        private void NavigateToEditEmployee()
        {
            _navigationService.NavigateTo("UpdateEmployee", Employee.Id);
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo("EmployeeList");
        }

        private async void AddEmployee(PasswordWithVerification passwordWithVerification)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
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

                if (!Employee.Validate())
                {
                    ValidationError = "De ingevoerde data klopt niet of is involledig.";
                    PopupIsOpen = true;
                    return;
                }

                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(passwordWithVerification.Password);

                await _employeeService.CreateEmployeeAsync(
                    Employee.Name,
                    Employee.Iban,
                    Employee.Account.Username,
                    Marshal.PtrToStringUni(valuePtr),
                    Employee.Account.Role,
                    Employee.Address,
                    Employee.ContactDetails);
                NavigateBack();
            }
            catch (Exception e)
            {
                ValidationError = $"Er is een fout opgetreden bij het opslaan van de medewerker ({e.GetType()})";
                PopupIsOpen = true;
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
                passwordWithVerification.Dispose();
            }
        }

        private async void UpdateEmployee()
        {
            try
            {
                await _employeeService.SaveChangesAsync();
                _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
            }
            catch (Exception e)
            {
                ValidationError = $"Er is een fout opgetreden bij het opslaan van de medewerker ({e.GetType()})";
                PopupIsOpen = true;
            }
        }

        private async void RemoveEmployee()
        {
            if (!CanDeleteEmployee)
                throw new InvalidOperationException("Cannot remove this customer");

            await _employeeService.RemoveEmployeeAsync(Employee.Id);
            NavigateBack();
        }
    }
}