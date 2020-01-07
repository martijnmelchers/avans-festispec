using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;

namespace Festispec.DomainServices.Services
{
    public class OfflineCustomerService : ICustomerService
    {
        private readonly ISyncService<Customer> _syncService;

        public OfflineCustomerService(ISyncService<Customer> syncService)
        {
            _syncService = syncService;
        }
        
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _syncService.GetAll().ToList();
        }

        public Customer GetCustomer(int customerId)
        {
            return _syncService.GetEntity(customerId);
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            return await _syncService.GetEntityAsync(customerId);
        }

        public Task<int> RemoveCustomerAsync(int customerId)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Customer> CreateCustomerAsync(string name, int kvkNr, Address address, ContactDetails contactDetails)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Customer> CreateCustomerAsync(Customer customer)
        {
            throw new System.InvalidOperationException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new System.InvalidOperationException();
        }

        public bool CanDeleteCustomer(Customer customer)
        {
            return false;
        }

        public void Sync()
        {
        }
    }
}