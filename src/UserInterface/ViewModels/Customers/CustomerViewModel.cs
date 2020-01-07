using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
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

        public CustomerViewModel(ICustomerService customerService, IFrameNavigationService navigationService, IGoogleMapsService googleMapsService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            if (_navigationService.Parameter is int customerId)
            {
                Customer = _customerService.GetCustomer(customerId);
                CanDeleteCustomer = Customer.Festivals.Count == 0 && Customer.ContactPersons.Count == 0;
                SaveCommand = new RelayCommand(UpdateCustomer);
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

            DeleteCommand = new RelayCommand(RemoveCustomer);
            OpenDeleteCheckCommand = new RelayCommand(() => DeletePopupIsOpen = true, CanDeleteCustomer);

            #region Google Search
            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            #endregion

        }


        private void NavigateToCustomerInfo() => _navigationService.NavigateTo("CustomerInfo", Customer.Id);
        private void NavigateToCustomerList() => _navigationService.NavigateTo("CustomerList");

        private async void AddCustomer()
        {
            try
            {
                await _customerService.CreateCustomerAsync(Customer);
                NavigateToCustomerList();
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
                await _customerService.SaveChangesAsync();
                NavigateToCustomerInfo();
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

        public async void Search()
        {
            try
            {
                Suggestions = new ObservableCollection<Prediction>(await _googleService.GetSuggestions(SearchQuery));
            }
            catch (GoogleMapsApiException)
            {
                MessageBox.Show("ERROR!");
            }

            RaisePropertyChanged(nameof(Suggestions));

        }

        public async void Select(string id)
        {
            try
            {
                var address = await _googleService.GetAddress(id);
                Customer.Address = address;
            }
            catch (GoogleMapsApiException)
            {
                MessageBox.Show("Error fetching place, try again!");
            }
        }

        #endregion
    }
}