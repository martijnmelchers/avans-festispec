using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEmployeeService _employeeService;
        private readonly IFrameNavigationService _navigationService;

        public Employee Employee { get; }

        public ICommand SaveCommand { get; }
        public ICommand RemoveEmployeeCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand EditEmployeeCommand { get; }

        public IEnumerable<Role> AvailableRoles => Enum.GetValues(typeof(Role)).OfType<Role>().ToList();

        public bool CanDeleteEmployee { get; }

        public ICommand EditAccountCommand { get; set; }

        public EmployeeViewModel(IEmployeeService employeeService, IFrameNavigationService navigationService)
        {
            _employeeService = employeeService;
            _navigationService = navigationService;

            if (_navigationService.Parameter is int customerId)
            {
                Employee = _employeeService.GetEmployee(customerId);
                CanDeleteEmployee = _employeeService.CanRemoveEmployee(Employee);
                SaveCommand = new RelayCommand(UpdateEmployee);
            }
            else
            {
                Employee = new Employee {Account = new Account()};
                SaveCommand = new RelayCommand<string>(AddEmployee);
            }

            CancelCommand = new RelayCommand(NavigateToAccount);
            RemoveEmployeeCommand = new RelayCommand(RemoveEmployee);
            EditEmployeeCommand = new RelayCommand(NavigateToEditEmployee);
            EditAccountCommand = new RelayCommand(NavigateToEditAccount);
            NavigateBackCommand = new RelayCommand(NavigateBack);
        }

        public ICommand NavigateBackCommand { get; }

        private void NavigateToAccount()
        {
            _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
        }

        private void NavigateToEditAccount()
        {
            _navigationService.NavigateTo("UpdateAccount", Employee.Id);
        }

        private void NavigateToEditEmployee()
        {
            _navigationService.NavigateTo("UpdateEmployee", Employee.Id);
        }


        private void NavigateBack()
        {
            _navigationService.NavigateTo("EmployeeList");
        }

        private async void AddEmployee(string password)
        {
            try
            {
                await _employeeService.CreateEmployeeAsync(
                    Employee.Name,
                    Employee.Iban,
                    Employee.Account.Username,
                    password,
                    Employee.Account.Role,
                    Employee.Address,
                    Employee.ContactDetails);
                NavigateBack();
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while adding an employee. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateEmployee()
        {
            try
            {
                await _employeeService.SaveChangesAsync();
                _navigationService.NavigateTo("EmployeeInfo", Employee.Id);
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occured while editing an employee. The occured error is: {e.GetType()}", $"{e.GetType()}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RemoveEmployee()
        {
            if (!CanDeleteEmployee)
                throw new InvalidOperationException("Cannot remove this customer");

            await _employeeService.RemoveEmployeeAsync(Employee.Id);
            NavigateBack();
        }
    }
}