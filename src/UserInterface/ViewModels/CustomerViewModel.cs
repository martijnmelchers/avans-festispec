using System;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class CustomerViewModel
    {
        private readonly ICustomerService _customerService;

        public ICommand SaveCommand { get; }

        public Customer Customer { get; }

        public CustomerViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
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