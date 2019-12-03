using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    class CustomerListViewModel
    {
        private readonly ICustomerService _customerService;
        private readonly IFrameNavigationService _navigationService;

        public ObservableCollection<Customer> CustomerList { get; }

        public ICommand AddNewCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand RemoveCustomerCommand { get; }

        public CustomerListViewModel(ICustomerService customerService, IFrameNavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            CustomerList = new ObservableCollection<Customer>();
            _customerService.GetAllCustomers().ForEach(c => CustomerList.Add(c));

            AddNewCustomerCommand = new RelayCommand(NavigateToAddNewCustomer);
            EditCustomerCommand = new RelayCommand<Customer>(NavigateToEditCustomer);
            RemoveCustomerCommand = new RelayCommand<Customer>(RemoveCustomer);
        }

        private void RemoveCustomer(Customer obj)
        {
            
        }

        private void NavigateToEditCustomer(Customer obj)
        {
            _navigationService.NavigateTo("EditCustomer", obj.Id);
        }

        private void NavigateToAddNewCustomer()
        {
            _navigationService.NavigateTo("NewCustomer");
        }
    }
}
