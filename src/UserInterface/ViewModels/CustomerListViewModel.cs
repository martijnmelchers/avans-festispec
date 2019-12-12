using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels
{
    public class CustomerListViewModel
    {
        private readonly IFrameNavigationService _navigationService;

        public CollectionView CustomerList { get; }

        public ICommand AddNewCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }

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

        public CustomerListViewModel(ICustomerService customerService, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            AddNewCustomerCommand = new RelayCommand(NavigateToAddNewCustomer);
            EditCustomerCommand = new RelayCommand<int>(NavigateToEditCustomer);

            CustomerList = (CollectionView)CollectionViewSource.GetDefaultView(customerService.GetAllCustomers());
            CustomerList.Filter = Filter;
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
