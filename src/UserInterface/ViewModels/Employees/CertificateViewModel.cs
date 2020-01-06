using System;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Exceptions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class CertificateViewModel : BaseDeleteCheckViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IFrameNavigationService _navigationService;

        public CertificateViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService)
        {
            _employeeService = employeeService;
            _navigationService = navigationService;

            switch (_navigationService.Parameter)
            {
                case int certificateId:
                    SaveCommand = new RelayCommand(UpdateCertificate);
                    Certificate = _employeeService.GetCertificate(certificateId);
                    break;

                case Employee employee:
                    SaveCommand = new RelayCommand(CreateCertificate);
                    Certificate = new Certificate {Employee = employee};
                    break;

                default:
                    throw new InvalidNavigationException();
            }

            EmployeeId = Certificate.Employee.Id;
            NavigateBackCommand = new RelayCommand(NavigateBack);
            DeleteCommand = new RelayCommand(RemoveCertificate);
            OpenDeleteCheckCommand = new RelayCommand(() => DeletePopupIsOpen = true);
        }

        public Certificate Certificate { get; }

        private int EmployeeId { get; }

        public ICommand SaveCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand OpenDeleteCheckCommand { get; }

        private async void RemoveCertificate()
        {
            await _employeeService.RemoveCertificateAsync(Certificate.Id);
            NavigateBack();
        }

        private void NavigateBack()
        {
            _navigationService.NavigateTo("CertificateList", EmployeeId);
        }

        private async void UpdateCertificate()
        {
            if (!Certificate.Validate())
            {
                ValidationError = "De ingevoerde data klopt niet of is involledig.";
                PopupIsOpen = true;
                return;
            }

            try
            {
                await _employeeService.SaveChangesAsync();
                _employeeService.Sync();
                NavigateBack();
            }
            catch (Exception e)
            {
                ValidationError = $"Er is een fout opgetreden bij het opslaan van het certificaat ({e.GetType()})";
                PopupIsOpen = true;
            }
        }

        private async void CreateCertificate()
        {
            if (!Certificate.Validate())
            {
                ValidationError = "De ingevoerde data klopt niet of is involledig.";
                PopupIsOpen = true;
                return;
            }

            try
            {
                // workaround for entity framework throwing an "Account is required" error
                Certificate.Employee = _employeeService.GetEmployee(EmployeeId);

                await _employeeService.CreateCertificateAsync(Certificate);
                _employeeService.Sync();
                NavigateBack();
            }
            catch (Exception e)
            {
                ValidationError = $"Er is een fout opgetreden bij het opslaan van het certificaat ({e.GetType()})";
                PopupIsOpen = true;
            }
        }
    }
}