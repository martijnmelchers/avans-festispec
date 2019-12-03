using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace Festispec.UI.ViewModels
{
    public class CustomerViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;

        public ICommand SaveCommand { get; }
        public ICommand RemoveCustomerCommand { get; }

        public Customer Customer { get; }

        public ObservableCollection<Customer> CustomerList { get; }

        public CustomerViewModel(ICustomerService customerService, IFrameNavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            CustomerList = new ObservableCollection<Customer>();
            _customerService.GetAllCustomers().ForEach(c => CustomerList.Add(c));
            
            Customer = new Customer();
            
            SaveCommand = new RelayCommand(AddCustomer);
        }

        private async void AddCustomer()
        {
            try
            {
                await _customerService.CreateCustomer(Customer);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding a question. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}