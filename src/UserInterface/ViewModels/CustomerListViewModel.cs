using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private int _tempRemovalId;

        public ObservableCollection<Customer> CustomerList { get; }

        public ICommand AddNewCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand RemoveCustomerCommand { get; }
        public ICommand StoreRemovalIdCommand { get; }
        public ICommand ClearRemovalIdCommand { get; }

        public CustomerListViewModel(ICustomerService customerService, IFrameNavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            CustomerList = new ObservableCollection<Customer>();
            _customerService.GetAllCustomers().ForEach(c => CustomerList.Add(c));

            AddNewCustomerCommand = new RelayCommand(NavigateToAddNewCustomer);
            EditCustomerCommand = new RelayCommand<int>(NavigateToEditCustomer);
            RemoveCustomerCommand = new RelayCommand(RemoveCustomer);
            StoreRemovalIdCommand = new RelayCommand<int>(StoreRemovalId);
            ClearRemovalIdCommand = new RelayCommand(ClearRemovalId);
        }

        private void StoreRemovalId(int customerId)
        {
            _tempRemovalId = customerId;
        }

        private void ClearRemovalId()
        {
            _tempRemovalId = 0;
        }

        private void RemoveCustomer()
        {
            if (_tempRemovalId == 0)
                throw new InvalidOperationException("Cannot remove a customer when the temporary ID is 0");

            _customerService.RemoveCustomer(_tempRemovalId);
            CustomerList.Remove(CustomerList.Single(customer => customer.Id == _tempRemovalId));
            ClearRemovalId();
        }

        private void NavigateToEditCustomer(int customerId)
        {
            _navigationService.NavigateTo("EditCustomer", customerId);
        }

        private void NavigateToAddNewCustomer()
        {
            _navigationService.NavigateTo("NewCustomer");
        }
    }
}
