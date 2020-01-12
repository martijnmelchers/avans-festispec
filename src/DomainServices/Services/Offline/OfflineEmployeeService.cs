using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services.Offline
{
    [ExcludeFromCodeCoverage]
    public class OfflineEmployeeService : IEmployeeService
    {
        private readonly ISyncService<Employee> _employeeSyncService;

        public OfflineEmployeeService(ISyncService<Employee> employeeSyncService)
        {
            _employeeSyncService = employeeSyncService;
        }
        public List<Employee> GetAllEmployees()
        {
            return _employeeSyncService.GetAll().Where(e => e.Account.IsNonActive == null).ToList();
        }

        public List<Employee> GetAllEmployeesActiveAndNonActive()
        {
            return _employeeSyncService.GetAll().ToList();
        }

        public Employee GetEmployee(int employeeId)
        {
            return _employeeSyncService.GetEntity(employeeId);
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            return await _employeeSyncService.GetEntityAsync(employeeId);
        }

        public Task<int> RemoveEmployeeAsync(int employeeId)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Employee> CreateEmployeeAsync(FullName name, string iban, string username, string password, Role role, Address address,
            ContactDetails contactDetails)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            throw new System.InvalidOperationException();
        }

        public Task UpdateEmployee(Employee employee)
        {
            throw new System.InvalidOperationException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new System.InvalidOperationException();
        }

        public bool CanRemoveEmployee(Employee employee)
        {
            return false;
        }

        public Account GetAccountForEmployee(int employeeId)
        {
            return GetEmployee(employeeId).Account.ToSafeAccount();
        }

        public Certificate GetCertificate(int certificateId)
        {
            foreach (var cert in GetAllEmployees()
                .Select(allEmployee => allEmployee.Certificates.FirstOrDefault(c => c.Id == certificateId))
                .Where(cert => cert != null))
            {
                return cert;
            }

            throw new EntityNotFoundException();
        }

        public Task<int> RemoveCertificateAsync(int certificateId)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Certificate> CreateCertificateAsync(Certificate certificate)
        {
            throw new System.InvalidOperationException();
        }

        public void Sync()
        {
        }
    }
}