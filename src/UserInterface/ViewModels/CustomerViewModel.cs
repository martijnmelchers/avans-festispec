using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class CustomerViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;

        public Customer Customer { get; }

        public ICommand SaveCommand { get; }
        public ICommand RemoveCustomerCommand { get; set; }
        public ICommand ReturnToCustomerListCommand { get; }

        public bool CanDeleteCustomer { get; }

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

            ReturnToCustomerListCommand = new RelayCommand(NavigateToCustomerList);
            RemoveCustomerCommand = new RelayCommand(RemoveCustomer);
        }


        private void NavigateToCustomerList()
        {
            _navigationService.NavigateTo("CustomerList");
        }

        private async void AddCustomer()
        {
            try
            {
                await _customerService.CreateCustomerAsync(Customer);
                NavigateToCustomerList();
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
                NavigateToCustomerList();
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
            NavigateToCustomerList();
        }
    }
}