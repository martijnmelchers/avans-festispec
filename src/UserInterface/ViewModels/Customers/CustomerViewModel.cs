using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Customers
{
    public class CustomerViewModel : BaseDeleteCheckViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IGoogleMapsService _googleService;

        public CustomerViewModel(ICustomerService customerService, IFrameNavigationService navigationService, IGoogleMapsService googleMapsService, IOfflineService offlineService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            if (_navigationService.Parameter is int customerId)
            {
                Customer = _customerService.GetCustomer(customerId);
                CanDeleteCustomer = Customer.Festivals.Count == 0 && Customer.ContactPersons.Count == 0;
                SaveCommand = new RelayCommand(UpdateCustomer);
                CurrentAddress = $"Huidige adres: {Customer.Address}";
            }
            else
            {
                Customer = new Customer();
                CanDeleteCustomer = false;
                SaveCommand = new RelayCommand(AddCustomer);
            }

            EditCustomerCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateCustomer", Customer.Id));
            AddFestivalCommand = new RelayCommand(() => _navigationService.NavigateTo("CreateFestival", Customer.Id));
            NavigateToCustomerListCommand = new RelayCommand(NavigateToCustomerList);
            NavigateToCustomerInfoCommand = new RelayCommand(NavigateToCustomerInfo);

            DeleteCommand = new RelayCommand(RemoveCustomer, () => offlineService.IsOnline);
            OpenDeleteCheckCommand = new RelayCommand(() => DeletePopupIsOpen = true, CanDeleteCustomer);
            
            customerService.Sync();

            #region Google Search
            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            #endregion
        }

        public Customer Customer { get; }
        public bool CanEditCustomer { get; }
        private bool CanDeleteCustomer { get; }

        public ICommand SaveCommand { get; }
        public ICommand NavigateToCustomerListCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand OpenDeleteCheckCommand { get; }
        public ICommand NavigateToCustomerInfoCommand { get; }
        public ICommand AddFestivalCommand { get; }
        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }


        private void NavigateToCustomerInfo() => _navigationService.NavigateTo("CustomerInfo", Customer.Id);
        private void NavigateToCustomerList() => _navigationService.NavigateTo("CustomerList");

        private async void AddCustomer()
        {
            try
            {
                await _customerService.CreateCustomerAsync(Customer);
                _customerService.Sync();
                NavigateToCustomerList();
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
                ValidationError = $"Er is een fout opgetreden bij het opslaan van de klant ({e.GetType()})";
                PopupIsOpen = true;
            }
        }

        private async void UpdateCustomer()
        {
            try
            {
                await _customerService.UpdateCustomerAsync(Customer);
                _customerService.Sync();
                NavigateToCustomerInfo();
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
                ValidationError = $"Er is een fout opgetreden bij het opslaan van de klant ({e.GetType()})";
                PopupIsOpen = true;
            }
        }

        private async void RemoveCustomer()
        {
            if (!CanDeleteCustomer)
                throw new InvalidOperationException("Cannot remove this customer");

            await _customerService.RemoveCustomerAsync(Customer.Id);
            NavigateToCustomerList();
        }

        #region Google Search
        public ObservableCollection<Prediction> Suggestions { get; set; }
        public string SearchQuery { get; set; }
        public string CurrentAddress { get; set; }

        public async void Search()
        {
            try
            {
                Suggestions = new ObservableCollection<Prediction>(await _googleService.GetSuggestions(SearchQuery ?? string.Empty));
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
                Customer.Address = address;
                CurrentAddress = $"Geselecteerde adres: {Customer.Address}";
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