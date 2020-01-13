using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface ICustomerService : ISyncable
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomer(int customerId);
        Task<Customer> GetCustomerAsync(int customerId);
        Task<int> RemoveCustomerAsync(int customerId);
        Task<Customer> CreateCustomerAsync(string name, int kvkNr, Address address, ContactDetails contactDetails);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        public bool CanDeleteCustomer(Customer customer);
    }
}