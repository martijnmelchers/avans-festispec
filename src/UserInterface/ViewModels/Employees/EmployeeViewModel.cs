using System;
using System.Windows;
using System.Windows.Input;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.UI.Interfaces;
using GalaSoft.MvvmLight.Command;

namespace Festispec.UI.ViewModels.Employees
{
    public class EmployeeViewModel
    {
        private readonly IEmployeeService _customerService;
        private readonly IFrameNavigationService _navigationService;

        public Employee Employee { get; }

        public ICommand SaveCommand { get; }
        public ICommand RemoveEmployeeCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand EditEmployeeCommand { get; }

        public bool CanDeleteEmployee { get; }

        public ICommand AddFestivalCommand { get; }

        public EmployeeViewModel(IEmployeeService customerService, IFrameNavigationService navigationService)
        {
            _customerService = customerService;
            _navigationService = navigationService;

            if (_navigationService.Parameter is int customerId)
            {
                Employee = _customerService.GetEmployee(customerId);
                CanDeleteEmployee = false; // TODO
                SaveCommand = new RelayCommand(UpdateEmployee);
            }
            else
            {
                Employee = new Employee();
                CanDeleteEmployee = false;
                SaveCommand = new RelayCommand(AddEmployee);
            }

            CancelCommand = new RelayCommand(NavigateBack);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployee);
            EditEmployeeCommand = new RelayCommand(NavigateToEditEmployee);
            AddFestivalCommand = new RelayCommand(NavigateToAddFestival);
        }

        private void NavigateToAddFestival()
        {
            _navigationService.NavigateTo("CreateFestival", Employee.Id);
        }

        private void NavigateToEditEmployee()
        {
            _navigationService.NavigateTo("UpdateEmployee", Employee.Id);
        }


        private void NavigateBack()
        {
            _navigationService.NavigateTo("EmployeeList");
        }

        private async void AddEmployee()
        {
            try
            {
                await _customerService.CreateEmployeeAsync(Employee);
                NavigateBack();
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding a customer. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateEmployee()
        {
            try
            {
                await _customerService.SaveChangesAsync();
                _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while editing a customer. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RemoveEmployee()
        {
            if (!CanDeleteEmployee)
                throw new InvalidOperationException("Cannot remove this customer");

            await _customerService.RemoveEmployeeAsync(Employee.Id);
            NavigateBack();
        }
    }
}