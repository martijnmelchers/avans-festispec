using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly FestispecContext _db;
        private readonly IAuthenticationService _authenticationService;

        public EmployeeService(FestispecContext db, IAuthenticationService authenticationService)
        {
            _db = db;
            _authenticationService = authenticationService;
        }

        public List<Employee> GetAllEmployees()
        {
            return _db.Employees.ToList();
        }

        public async Task<Employee> CreateEmployeeAsync(FullName name, string iban, string username, string password,
            Role role, Address address, ContactDetails contactDetails)
        {
            Account account = await _authenticationService.Register(username, password, role);

            var employee = new Employee
            {
                Name = name,
                Iban = iban,
                Account = account,
                Address = address,
                ContactDetails = contactDetails
            };
            
            return await CreateEmployeeAsync(employee);
        }
        
        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            if (!employee.Validate())
                throw new InvalidDataException();

            _db.Employees.Add(employee);

            await SaveChangesAsync();

            return employee;
        }
        
        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            Employee employee = await _db.Employees
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new EntityNotFoundException();

            return employee;
        }
        
        public Employee GetEmployee(int employeeId)
        {
            Employee employee = _db.Employees
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
                throw new EntityNotFoundException();

            return employee;
        }

        public async Task<int> RemoveEmployeeAsync(int employeeId)
        {
            Employee employee = await GetEmployeeAsync(employeeId);
            
            _db.Employees.Remove(employee);
            _db.Accounts.Remove(employee.Account);

            return await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
