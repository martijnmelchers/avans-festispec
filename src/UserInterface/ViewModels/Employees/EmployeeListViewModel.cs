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
        private string _search;

        public EmployeeListViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService, IOfflineService offlineService)
        {

            AddNewEmployeeCommand = new RelayCommand(() => navigationService.NavigateTo("CreateEmployee"), () => offlineService.IsOnline, true);
            ViewEmployeeCommand = new RelayCommand<int>(employeeId => navigationService.NavigateTo("EmployeeInfo", employeeId));

            EmployeeList =
                (CollectionView) CollectionViewSource.GetDefaultView(
                    employeeService.GetAllEmployeesActiveAndNonActive());
            EmployeeList.Filter = Filter;
            
            employeeService.Sync();
        }

        public CollectionView EmployeeList { get; }

        public ICommand AddNewEmployeeCommand { get; }
        public ICommand ViewEmployeeCommand { get; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                EmployeeList.Filter += Filter;
            }
        }

        private bool Filter(object item)
        {
            return string.IsNullOrEmpty(Search) ||
                   ((Employee) item).Name.ToString().IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}