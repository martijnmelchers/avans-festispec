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
        private string _search;

        public CustomerListViewModel(ICustomerService customerService, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            AddNewCustomerCommand = new RelayCommand(NavigateToAddCustomer);
            ViewCustomerCommand = new RelayCommand<int>(NavigateToViewCustomer);

            CustomerList = (CollectionView) CollectionViewSource.GetDefaultView(customerService.GetAllCustomers());
            CustomerList.Filter = Filter;
        }

        public CollectionView CustomerList { get; }

        public ICommand AddNewCustomerCommand { get; }
        public ICommand ViewCustomerCommand { get; }
        
        private void NavigateToAddCustomer() => _navigationService.NavigateTo("CreateCustomer");
        private void NavigateToViewCustomer(int customerId) => _navigationService.NavigateTo("CustomerInfo", customerId);

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CustomerList.Filter += Filter;
            }
        }

        private bool Filter(object item) =>
            string.IsNullOrEmpty(Search) ||
            ((Customer) item).CustomerName.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}