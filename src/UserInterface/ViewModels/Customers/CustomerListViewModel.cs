using System;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Customers
{
    public class CustomerListViewModel
    {
        private readonly IFrameNavigationService _navigationService;

        public CollectionView CustomerList { get; }

        public ICommand AddNewCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand ViewCustomerCommand { get; }

        private bool Filter(object item) => string.IsNullOrEmpty(Search) || ((Customer)item).CustomerName.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;

        private string _search;

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CustomerList.Filter += Filter;
            }
        }

        

        public CustomerListViewModel(ICustomerService customerService, IFrameNavigationService navigationService, OfflineService offlineService)
        {
            _navigationService = navigationService;

            AddNewCustomerCommand = new RelayCommand(NavigateToAddNewCustomer);
            EditCustomerCommand = new RelayCommand<int>(NavigateToEditCustomer);
            ViewCustomerCommand = new RelayCommand<int>(NavigateToViewCustomer);

            CustomerList = (CollectionView)CollectionViewSource.GetDefaultView(customerService.GetAllCustomers());
            CustomerList.Filter = Filter;

            CanEditCustomers = offlineService.IsOnline;
        }

        public bool CanEditCustomers { get; }

        private void NavigateToViewCustomer(int customerId)
        {
            _navigationService.NavigateTo("CustomerInfo", customerId);
        }

        private void NavigateToEditCustomer(int customerId)
        {
            _navigationService.NavigateTo("UpdateCustomer", customerId);
        }

        private void NavigateToAddNewCustomer()
        {
            _navigationService.NavigateTo("CreateCustomer");
        }
    }
}
