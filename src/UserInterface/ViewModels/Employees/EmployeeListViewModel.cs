using System;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class EmployeeListViewModel
    {
        private readonly IFrameNavigationService _navigationService;
        private string _search;

        public EmployeeListViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService, OfflineService offlineService)
        {
            _navigationService = navigationService;

            AddNewEmployeeCommand = new RelayCommand(NavigateToAddNewEmployee);
            ViewEmployeeCommand = new RelayCommand<int>(NavigateToViewEmployee);

            EmployeeList = (CollectionView) CollectionViewSource.GetDefaultView(employeeService.GetAllEmployees());
            EmployeeList.Filter = Filter;

            CanEditEmployees = offlineService.IsOnline;
            
            employeeService.Sync();
        }

        public CollectionView EmployeeList { get; }
        
        public bool CanEditEmployees { get; }

        public ICommand AddNewEmployeeCommand { get; }
        public ICommand ViewEmployeeCommand { get; }

        private void NavigateToAddNewEmployee() => _navigationService.NavigateTo("CreateEmployee");
        private void NavigateToViewEmployee(int employeeId) => _navigationService.NavigateTo("EmployeeInfo", employeeId);

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                EmployeeList.Filter += Filter;
            }
        }

        private bool Filter(object item) =>
            string.IsNullOrEmpty(Search) ||
            ((Employee) item).Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}