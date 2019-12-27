using System;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Customers
{
    public class CustomerViewModel : BaseDeleteCheckViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;

        public Customer Customer { get; }

        public ICommand SaveCommand { get; }
        public ICommand RemoveCustomerCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand EditCustomerCommand { get; }

        public bool CanDeleteCustomer { get; }

        public ICommand AddFestivalCommand { get; }

        public CustomerViewModel(ICustomerService customerService, IFrameNavigationService navigationService)
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
                _navigationService.NavigateTo("CustomerInfo", Customer.Id);
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
            NavigateBack();
        }
    }
}