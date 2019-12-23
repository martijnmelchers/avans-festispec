using System;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Exceptions;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class CertificateViewModel : BaseValidationViewModel
    {
        private readonly IFrameNavigationService _navigationService;
        private readonly IEmployeeService _employeeService;

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
            RemoveCommand = new RelayCommand(RemoveCertificate);
        }

        public Certificate Certificate { get; }

        private int EmployeeId { get; }

        public ICommand SaveCommand { get; }

        public ICommand NavigateBackCommand { get; }

        public ICommand RemoveCommand { get; }

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
                // workaround for entity framework not having brain cells
                // AcCoUnT iS ReQuIrEd
                Certificate.Employee = _employeeService.GetEmployee(EmployeeId);
                
                await _employeeService.CreateCertificateAsync(Certificate);
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