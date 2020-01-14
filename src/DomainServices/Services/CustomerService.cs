using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly FestispecContext _db;
        private readonly ISyncService<Customer> _syncService;
        private readonly IAddressService _addressService;

        public CustomerService(FestispecContext db, ISyncService<Customer> syncService, IAddressService addressService)
        {
            _db = db;
            _syncService = syncService;
            _addressService = addressService;
        }

        public List<Customer> GetAllCustomers() => _db.Customers.Include(c => c.Address).ToList();

        public async Task<Customer> CreateCustomerAsync(string name, int kvkNr, Address address,
            ContactDetails contactDetails)
        {
            var customer = new Customer
            {
                CustomerName = name,
                KvkNr = kvkNr,
                Address = address,
                ContactDetails = contactDetails
            };

            return await CreateCustomerAsync(customer);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (!customer.Validate() || !customer.ContactDetails.Validate())
                throw new InvalidDataException();

            customer.Address = await _addressService.SaveAddress(customer.Address);

            _db.Customers.Add(customer);

            await SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            var customer = await _db.Customers
                .Include(c => c.Festivals)
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                throw new EntityNotFoundException();

            return customer;
        }

        public Customer GetCustomer(int customerId)
        {
            var customer = _db.Customers
                .Include(c => c.Festivals)
                .Include(c => c.Address)
                .FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                throw new EntityNotFoundException();

            return customer;
        }

        public async Task<int> RemoveCustomerAsync(int customerId)
        {
            var customer = await GetCustomerAsync(customerId);

            if (customer.Festivals?.Count > 0)
                throw new CustomerHasFestivalsException();

            await _addressService.RemoveAddress(customer.Address);
            _db.Customers.Remove(customer);

            return await SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (!customer.Validate() || !customer.ContactDetails.Validate())
                throw new InvalidDataException();

            customer.Address = await _addressService.SaveAddress(customer.Address);

            await SaveChangesAsync();
        }

        private async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

        public bool CanDeleteCustomer(Customer customer) => customer.Festivals.Count == 0;

        [ExcludeFromCodeCoverage]
        public void Sync()
        {
            var db = _syncService.GetSyncContext();
        
            var customers = db.Customers
                .Include(c => c.Address)
                .Include(c => c.Festivals).ToList();
            
            _syncService.Flush();
            _syncService.AddEntities(customers);
            _syncService.SaveChanges();
        }
    }
}