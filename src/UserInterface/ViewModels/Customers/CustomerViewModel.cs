using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;

namespace Festispec.UI.ViewModels.Customers
{
    public class CustomerViewModel : BaseDeleteCheckViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IGoogleMapsService _googleService;
        private readonly IFrameNavigationService _navigationService;

        public CustomerViewModel(ICustomerService customerService, IFrameNavigationService navigationService,
            IGoogleMapsService googleMapsService, IOfflineService offlineService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            if (_navigationService.Parameter is int customerId)
            {
                Customer = _customerService.GetCustomer(customerId);
                CanDeleteCustomer = _customerService.CanDeleteCustomer(Customer);
                SaveCommand = new RelayCommand(UpdateCustomer);
                CurrentAddress = $"Huidige adres: {Customer.Address}";
            }
            else
            {
                Customer = new Customer();
                CanDeleteCustomer = false;
                SaveCommand = new RelayCommand(AddCustomer);
            }

            EditCustomerCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateCustomer", Customer.Id),
                () => offlineService.IsOnline, true);
            AddFestivalCommand = new RelayCommand(() => _navigationService.NavigateTo("CreateFestival", Customer.Id),
                () => offlineService.IsOnline, true);
            NavigateToCustomerListCommand = new RelayCommand(() => _navigationService.NavigateTo("CustomerList"));
            NavigateToCustomerInfoCommand = new RelayCommand(() => _navigationService.NavigateTo("CustomerInfo", Customer.Id));

            DeleteCommand = new RelayCommand(RemoveCustomer, () => offlineService.IsOnline, true);
            OpenDeleteCheckCommand = new RelayCommand(OpenDeletePopup, () => CanDeleteCustomer, true);
            
            customerService.Sync();

            #region Google Search

            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);

            #endregion
        }

        public Customer Customer { get; }
        private bool CanDeleteCustomer { get; }

        public ICommand SaveCommand { get; }
        public ICommand NavigateToCustomerListCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand OpenDeleteCheckCommand { get; }
        public ICommand NavigateToCustomerInfoCommand { get; }
        public ICommand AddFestivalCommand { get; }
        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }

        private async void AddCustomer()
        {
            if (string.IsNullOrEmpty(CurrentAddress))
            {
                OpenValidationPopup("Er is geen adres ingevuld.");
                return;
            }

            try
            {
                await _customerService.CreateCustomerAsync(Customer);
                _customerService.Sync();
                _navigationService.NavigateTo("CustomerList");
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
                OpenValidationPopup($"Er is een fout opgetreden bij het opslaan van de klant ({e.GetType()})");
            }
        }

        private async void UpdateCustomer()
        {
            try
            {
                await _customerService.UpdateCustomerAsync(Customer);
                _customerService.Sync();
                _navigationService.NavigateTo("CustomerInfo", Customer.Id);
            }
            catch (InvalidAddressException)
            {
                OpenValidationPopup("Er is een ongeldig adres ingevoerd, controleer of je minimaal een straat, postcode en plaats hebt.");
            }
            catch (InvalidDataException)
            {
                OpenValidationPopup("De ingevoerde data klopt niet of is involledig.");
            }
            catch (Exception e)
            {
                OpenValidationPopup($"Er is een fout opgetreden bij het opslaan van de klant ({e.GetType()})");
            }
        }

        private async void RemoveCustomer()
        {
            if (!CanDeleteCustomer)
                throw new InvalidOperationException("Cannot remove this customer");

            await _customerService.RemoveCustomerAsync(Customer.Id);
            _navigationService.NavigateTo("CustomerList");
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
                OpenValidationPopup("Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
            catch (GoogleZeroResultsException)
            {
                OpenValidationPopup("Er zijn geen resultaten gevonden voor je zoekopdracht, wijzig je opdracht en probeer het opnieuw.");
            }
        }

        private async void Select(string id)
        {
            try
            {
                var address = await _googleService.GetAddress(id);
                Customer.Address = address;
                CurrentAddress = $"Geselecteerde adres: {Customer.Address}";
                RaisePropertyChanged(nameof(CurrentAddress));
            }
            catch (GoogleMapsApiException)
            {
                OpenValidationPopup("Er is een fout opgetreden tijdens het communiceren met Google Maps. Controleer of je toegang tot het internet hebt of neem contact op met je systeemadministrator");
            }
        }

        #endregion
    }
}