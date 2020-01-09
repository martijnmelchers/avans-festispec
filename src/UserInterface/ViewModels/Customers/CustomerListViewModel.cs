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
        private string _search;

        public CustomerListViewModel(ICustomerService customerService, IFrameNavigationService navigationService, IOfflineService offlineService)
        {

            AddNewCustomerCommand = new RelayCommand(() => navigationService.NavigateTo("CreateCustomer"), () => offlineService.IsOnline, true);
            ViewCustomerCommand = new RelayCommand<int>(customerId => navigationService.NavigateTo("CustomerInfo", customerId));

            CustomerList = (CollectionView) CollectionViewSource.GetDefaultView(customerService.GetAllCustomers());
            CustomerList.Filter = Filter;
            customerService.Sync();
        }

        public CollectionView CustomerList { get; }

        public ICommand AddNewCustomerCommand { get; }
        public ICommand ViewCustomerCommand { get; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CustomerList.Filter += Filter;
            }
        }

        private bool Filter(object item)
        {
            return string.IsNullOrEmpty(Search) ||
                   ((Customer) item).CustomerName.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}