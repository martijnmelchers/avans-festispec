using System;
using System.Windows.Data;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
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

        public CertificateListViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;

            if (!(_navigationService.Parameter is int employeeId))
                throw new InvalidNavigationException();

            Employee = employeeService.GetEmployee(employeeId);
            AddNewCertificateCommand = new RelayCommand(NavigateToAddNewCertificate);
            EditCertificateCommand = new RelayCommand<int>(NavigateToEditCertificate);
            NavigateBackCommand = new RelayCommand(NavigateBack);

            CertificateList = (CollectionView) CollectionViewSource.GetDefaultView(Employee.Certificates);
            CertificateList.Filter = Filter;
        }

        public CollectionView CertificateList { get; }

        public ICommand AddNewCertificateCommand { get; }
        public ICommand EditCertificateCommand { get; }

        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CertificateList.Filter += Filter;
            }
        }

        public ICommand NavigateBackCommand { get; }

        public Employee Employee { get; }

        private bool Filter(object item) =>
            string.IsNullOrEmpty(Search) ||
            ((Certificate) item).CertificateTitle.IndexOf(Search, StringComparison.OrdinalIgnoreCase) >= 0;

        private void NavigateBack()
        {
            _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
        }

        private void NavigateToEditCertificate(int certificateId)
        {
            _navigationService.NavigateTo("UpdateCertificate", certificateId);
        }

        private void NavigateToAddNewCertificate()
        {
            _navigationService.NavigateTo("CreateCertificate", Employee);
        }
    }
}