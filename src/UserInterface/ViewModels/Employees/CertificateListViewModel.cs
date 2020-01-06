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

        public CertificateListViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService, OfflineService offlineService)
        {
            if (!(navigationService.Parameter is int employeeId))
                throw new InvalidNavigationException();

            _navigationService = navigationService;

            Employee = employeeService.GetEmployee(employeeId);
            CanEditCertificates = offlineService.IsOnline;
            AddNewCertificateCommand = new RelayCommand(NavigateToAddNewCertificate);
            NavigateToEmployeeInfoCommand = new RelayCommand(NavigateToEmployeeInfo);
            EditCertificateCommand = new RelayCommand<int>(NavigateToEditCertificate);

            CertificateList = (CollectionView) CollectionViewSource.GetDefaultView(Employee.Certificates);
            CertificateList.Filter = Filter;
            
            employeeService.Sync();
        }

        public Employee Employee { get; }
        public bool CanEditCertificates { get; }

        public CollectionView CertificateList { get; }

        public ICommand AddNewCertificateCommand { get; }
        public ICommand EditCertificateCommand { get; }
        public ICommand NavigateToEmployeeInfoCommand { get; }
        
        private void NavigateToAddNewCertificate() => _navigationService.NavigateTo("CreateCertificate", Employee);
        private void NavigateToEmployeeInfo() => _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
        private void NavigateToEditCertificate(int certificateId) => _navigationService.NavigateTo("UpdateCertificate", certificateId);

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CertificateList.Filter += Filter;
            }
        }

        private bool Filter(object item) =>
            string.IsNullOrEmpty(Search) ||
            ((Certificate) item).CertificateTitle.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;
    }
}