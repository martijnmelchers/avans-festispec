using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Google;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Customers
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;
        private readonly IGoogleMapsService _googleService;

        public Customer Customer { get; }

        public ICommand SaveCommand { get; }
        public ICommand RemoveCustomerCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand SearchCommand { get; }
        public RelayCommand<string> SelectCommand { get; }

        public bool CanDeleteCustomer { get; }

        public ICommand AddFestivalCommand { get; }

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

            CancelCommand = new RelayCommand(NavigateBack);
            RemoveCustomerCommand = new RelayCommand(RemoveCustomer);
            EditCustomerCommand = new RelayCommand(NavigateToEditCustomer);
            AddFestivalCommand = new RelayCommand(NavigateToAddFestival);

            #region Google Search
            _googleService = googleMapsService;
            SearchCommand = new RelayCommand(Search);
            SelectCommand = new RelayCommand<string>(Select);
            #endregion
        }

        private void NavigateToAddFestival()
        {
            _navigationService.NavigateTo("CreateFestival", Customer.Id);
        }

        private void NavigateToEditCustomer()
        {
            _navigationService.NavigateTo("UpdateCustomer", Customer.Id);
        }


        private void NavigateBack()
        {
            _navigationService.NavigateTo("CustomerList");
        }

        private async void AddCustomer()
        {
            try
            {
                await _customerService.CreateCustomerAsync(Customer);
                NavigateBack();
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding a customer. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateCustomer()
        {
            try
            {
                await _customerService.SaveChangesAsync();
                _navigationService.NavigateTo("CustomerInfo", Customer.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while editing a customer. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RemoveCustomer()
        {
            if (!CanDeleteCustomer)
                throw new InvalidOperationException("Cannot remove this customer");

            await _customerService.RemoveCustomerAsync(Customer.Id);
            NavigateBack();
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