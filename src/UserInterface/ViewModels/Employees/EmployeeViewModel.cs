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
        private readonly IGoogleMapsService _googleService;
        private readonly IFrameNavigationService _navigationService;

        public EmployeeViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService,
            IGoogleMapsService googleMapsService, IOfflineService offlineService)
        {
            _employeeService = employeeService;
            _navigationService = navigationService;
            ViewCertificatesCommand = new RelayCommand(() => _navigationService.NavigateTo("CertificateList", Employee.Id));

            if (_navigationService.Parameter is int customerId)
            {
                Employee = _employeeService.GetEmployee(customerId);
                CanDeleteEmployee = _employeeService.CanRemoveEmployee(Employee);
                SaveCommand = new RelayCommand(UpdateEmployee);
                CurrentAddress = $"Huidige adres: {Employee.Address}";
            }
            else
            {
                Employee = new Employee
                {
                    Account = new Account(),
                    Name = new FullName(),
                    ContactDetails = new ContactDetails()
                };
                SaveCommand = new RelayCommand<PasswordWithVerification>(AddEmployee);
            }

            CancelCommand = new RelayCommand(() => _navigationService.NavigateTo("EmployeeInfo", Employee.Id));
            EditEmployeeCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateEmployee", Employee.Id), () => offlineService.IsOnline, true);
            EditAccountCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateAccount", Employee.Id), () => offlineService.IsOnline, true);
            NavigateBackCommand = new RelayCommand(NavigateBack);

            DeleteCommand = new RelayCommand(RemoveEmployee);
            OpenDeleteCheckCommand = new RelayCommand(OpenDeletePopup, () => CanDeleteEmployee, true);

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

        private void NavigateBack()
        {
            _navigationService.NavigateTo("EmployeeList");
        }

        private async void AddEmployee(PasswordWithVerification passwordWithVerification)
        {
            if (string.IsNullOrEmpty(CurrentAddress))
            {
                OpenValidationPopup("Er is geen adres ingevuld.");
                return;
            }

            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                if (!passwordWithVerification.Equal() || passwordWithVerification.Empty())
                {
                    OpenValidationPopup("Er is geen wachtwoord ingevuld of de wachtwoorden komen niet overeen.");
                    return;
                }

                if (passwordWithVerification.Password.Length < 5)
                {
                    OpenValidationPopup("Het wachtwoord moet tussen 5 en 100 karakters zijn.");
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
                _employeeService.Sync();
                NavigateBack();
            }
            catch (InvalidAddressException)
            {
                OpenValidationPopup(
                    "Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (InvalidDataException)
            {
                OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
            }
            catch (Exception e)
            {
                OpenValidationPopup($"Er is een fout opgetreden bij het opslaan van de medewerker ({e.GetType()})");
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
                _employeeService.Sync();
                _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
            }
            catch (InvalidAddressException)
            {
                OpenValidationPopup(
                    "Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (Exception e)
            {
                OpenValidationPopup($"Er is een fout opgetreden bij het opslaan van de medewerker ({e.GetType()})");
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
                Suggestions =
                    new ObservableCollection<Prediction>(
                        await _googleService.GetSuggestions(SearchQuery ?? string.Empty));
                RaisePropertyChanged(nameof(Suggestions));
            }
            catch (GoogleMapsApiException)
            {
                OpenValidationPopup(
                    "Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
            catch (GoogleZeroResultsException)
            {
                OpenValidationPopup(
                    "Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.");
            }
        }

        public async void Select(string id)
        {
            try
            {
                Address address = await _googleService.GetAddress(id);
                Employee.Address = address;
                CurrentAddress = $"Geselecteerde adres: {Employee.Address}";
                RaisePropertyChanged(nameof(CurrentAddress));
            }
            catch (GoogleMapsApiException)
            {
                OpenValidationPopup(
                    "Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
        }

        #endregion
    }
}