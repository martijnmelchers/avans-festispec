using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        
        Employee GetEmployee(int employeeId);
        Task<Employee> GetEmployeeAsync(int employeeId);
        
        Task<int> RemoveEmployeeAsync(int employeeId);

        Task<Employee> CreateEmployeeAsync(FullName name, string iban, string username, string password,
            Role role, Address address, ContactDetails contactDetails);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        
        Task<int> SaveChangesAsync();
        bool CanRemoveEmployee(Employee employee);
        Account GetAccountForEmployee(int employeeId);
    }
}