using System;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Customers
{
    public class CustomerViewModel : BaseDeleteCheckViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;

        public CustomerViewModel(ICustomerService customerService, IFrameNavigationService navigationService, IOfflineService offlineService)
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

            CanEditCustomer = offlineService.IsOnline;
            EditCustomerCommand = new RelayCommand(() => _navigationService.NavigateTo("UpdateCustomer", Customer.Id));
            AddFestivalCommand = new RelayCommand(() => _navigationService.NavigateTo("CreateFestival", Customer.Id));
            NavigateToCustomerListCommand = new RelayCommand(NavigateToCustomerList);
            NavigateToCustomerInfoCommand = new RelayCommand(NavigateToCustomerInfo);

            DeleteCommand = new RelayCommand(RemoveCustomer);
            OpenDeleteCheckCommand = new RelayCommand(() => DeletePopupIsOpen = true, CanDeleteCustomer);
            
            customerService.Sync();
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
                _customerService.Sync();
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
    }
}