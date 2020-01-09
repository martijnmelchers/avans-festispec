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
        private readonly ISyncService<Employee> _employeeSyncService;
        private readonly IAddressService _addressService;

        public EmployeeService(FestispecContext db, IAuthenticationService authenticationService, ISyncService<Employee> employeeSyncService, IAddressService addressService)
        {
            _db = db;
            _authenticationService = authenticationService;
            _employeeSyncService = employeeSyncService;
            _addressService = addressService;
        }

        public List<Employee> GetAllEmployees() //returns all active accounts.
        {
            return _db.Employees.Where(e => e.Account.IsNonActive == null).Include(e => e.Address).ToList();
        }

        public List<Employee> GetAllEmployeesActiveAndNonActive()
        {
            return _db.Employees
                .Include(e => e.Address)
                .Include(e => e.PlannedEvents)
                .ToList();
        }

        public async Task<Employee> CreateEmployeeAsync(FullName name, string iban, string username, string password,
            Role role, Address address, ContactDetails contactDetails)
        {
            Account account = _authenticationService.AssembleAccount(username, password, role);

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

            employee.Address = await _addressService.SaveAddress(employee.Address);

            _db.Employees.Add(employee);

            await SaveChangesAsync();

            return employee;
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            Employee employee = await _db.Employees
                .Include(e => e.Address)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                throw new EntityNotFoundException();

            return employee;
        }

        public Employee GetEmployee(int employeeId)
        {
            Employee employee = _db.Employees
                .Include(e => e.Address)
                .FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
                throw new EntityNotFoundException();

            return employee;
        }

        public Account GetAccountForEmployee(int employeeId)
        {
            Account account = _db.Accounts.FirstOrDefault(a => a.Id == employeeId);

            if (account == null)
                throw new EntityNotFoundException();

            return account;
        }

        public bool CanRemoveEmployee(Employee employee)
        {
            return employee.PlannedEvents.ToList().Count == 0;
        }

        public async Task<int> RemoveEmployeeAsync(int employeeId)
        {
            Employee employee = await GetEmployeeAsync(employeeId);

            if (employee.PlannedEvents.ToList().Count > 0)
                throw new EmployeeHasPlannedEventsException();

            await _addressService.RemoveAddress(employee.Address);
            _db.Accounts.Remove(employee.Account);
            _db.Employees.Remove(employee);

            return await SaveChangesAsync();
        }

        public async Task UpdateEmployee(Employee employee)
        {
            if (!employee.Validate())
                throw new InvalidDataException();

            employee.Address = await _addressService.SaveAddress(employee.Address);

            await SaveChangesAsync();
        }

        #region Certificate code

        public async Task<Certificate> CreateCertificateAsync(Certificate certificate)
        {
            if (!certificate.Validate())
                throw new InvalidDataException();

            _db.Certificates.Add(certificate);

            await SaveChangesAsync();

            return certificate;
        }

        public Certificate GetCertificate(int certificateId)
        {
            Certificate certificate = _db.Certificates.FirstOrDefault(a => a.Id == certificateId);

            if (certificate == null)
                throw new EntityNotFoundException();

            return certificate;
        }

        public async Task<int> RemoveCertificateAsync(int certificateId)
        {
            Certificate certificate = GetCertificate(certificateId);

            _db.Certificates.Remove(certificate);

            return await SaveChangesAsync();
        }

        #endregion Certificate code

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Sync()
        {
            FestispecContext db = _employeeSyncService.GetSyncContext();
        
            List<Employee> employees = db.Employees
                .Include(e => e.Certificates)
                .Include(e => e.Account).ToList();

            employees.ForEach(e => e.Account.ToSafeAccount());
            
            _employeeSyncService.Flush();
            _employeeSyncService.AddEntities(employees);
            _employeeSyncService.SaveChanges();
        }
    }
}