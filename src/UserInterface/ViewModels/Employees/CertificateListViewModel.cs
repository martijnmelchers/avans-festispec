using System;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.UI.Exceptions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class CertificateListViewModel
    {
        private readonly IFrameNavigationService _navigationService;
        private string _search;

        public CertificateListViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService,
            IOfflineService offlineService)
        {
            if (!(navigationService.Parameter is int employeeId))
                throw new InvalidNavigationException();

            _navigationService = navigationService;

            Employee = employeeService.GetEmployee(employeeId);
            AddNewCertificateCommand =
                new RelayCommand(() => _navigationService.NavigateTo("CreateCertificate", Employee),
                    () => offlineService.IsOnline, true);
            NavigateToEmployeeInfoCommand =
                new RelayCommand(() => _navigationService.NavigateTo("EmployeeInfo", Employee.Id));
            EditCertificateCommand =
                new RelayCommand<int>(
                    certificateId => _navigationService.NavigateTo("UpdateCertificate", certificateId),
                    _ => offlineService.IsOnline, true);

            CertificateList = (CollectionView) CollectionViewSource.GetDefaultView(Employee.Certificates);
            CertificateList.Filter = Filter;

            employeeService.Sync();
        }

        public Employee Employee { get; }

        public CollectionView CertificateList { get; }

        public ICommand AddNewCertificateCommand { get; }
        public ICommand EditCertificateCommand { get; }
        public ICommand NavigateToEmployeeInfoCommand { get; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CertificateList.Filter += Filter;
            }
        }

        private bool Filter(object item)
        {
            return string.IsNullOrEmpty(Search) ||
                   ((Certificate) item).CertificateTitle.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}