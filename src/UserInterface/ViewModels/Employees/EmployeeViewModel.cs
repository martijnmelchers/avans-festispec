using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class EmployeeViewModel : BaseDeleteCheckViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IGoogleMapsService _googleService;
        public EmployeeViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService, IGoogleMapsService googleMapsService)
        {
            _employeeService = employeeService;
            _navigationService = navigationService;
            ViewCertificatesCommand = new RelayCommand(ViewCertificates);

            if (_navigationService.Parameter is int customerId)
            {
                Employee = _employeeService.GetEmployee(customerId);
                CanDeleteEmployee = _employeeService.CanRemoveEmployee(Employee);
                SaveCommand = new RelayCommand(UpdateEmployee);
                CurrentAddress = $"Huidige adres: {Employee.Address.ToString()}";
            }
            else
            {
                Employee = new Employee { Account = new Account() };
                SaveCommand = new RelayCommand<PasswordWithVerification>(AddEmployee);
            }

            CancelCommand = new RelayCommand(() => _navigationService.NavigateTo("EmployeeInfo", Employee.Id));
            EditEmployeeCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateEmployee", Employee.Id));
            EditAccountCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateAccount", Employee.Id));
            NavigateBackCommand = new RelayCommand(NavigateBack);

            DeleteCommand = new RelayCommand(RemoveEmployee);
            OpenDeleteCheckCommand = new RelayCommand(() => DeletePopupIsOpen = true, CanDeleteEmployee);

            #region Google Search
            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            #endregion
        }

        public Employee Employee { get; }
        private bool CanDeleteEmployee { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditEmployeeCommand { get; }
        public ICommand EditAccountCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand ViewCertificatesCommand { get; }
        public ICommand OpenDeleteCheckCommand { get; }
        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }

        private void ViewCertificates()
        {
            _navigationService.NavigateTo("CertificateList", Employee.Id);
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
            catch (InvalidAddressException)
            {
                ValidationError = "Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.";
                PopupIsOpen = true;
            }
            catch (InvalidDataException)
            {
                ValidationError = "De ingevoerde data klopt niet of is involledig.";
                PopupIsOpen = true;
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
                await _employeeService.UpdateEmployee(Employee);
                _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
            }
            catch(InvalidAddressException)
            {
                ValidationError = "Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.";
                PopupIsOpen = true;
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
                throw new InvalidOperationException("Cannot remove this employee");

            await _employeeService.RemoveEmployeeAsync(Employee.Id);
            NavigateBack();
        }

        #region Google Search
        public ObservableCollection<Prediction> Suggestions { get; set; }
        public string SearchQuery { get; set; }
        public string CurrentAddress { get; set; }

        public async void Search()
        {
            try
            {
                Suggestions = new ObservableCollection<Prediction>(await _googleService.GetSuggestions(SearchQuery));
                RaisePropertyChanged(nameof(Suggestions));
            }
            catch (GoogleMapsApiException)
            {
                ValidationError = "Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator";
                PopupIsOpen = true;
            }
            catch (GoogleZeroResultsException)
            {
                ValidationError = "Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.";
                PopupIsOpen = true;
            }



        }

        public async void Select(string id)
        {
            try
            {
                var address = await _googleService.GetAddress(id);
                Employee.Address = address;
                CurrentAddress = $"Geselecteerde adres: {Employee.Address}";
                RaisePropertyChanged(nameof(CurrentAddress));
            }
            catch (GoogleMapsApiException)
            {
                ValidationError = "Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator";
                PopupIsOpen = true;
            }
        }

        #endregion
    }
}