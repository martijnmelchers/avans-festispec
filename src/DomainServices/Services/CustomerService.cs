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
    public class CustomerService : ICustomerService
    {
        private readonly FestispecContext _db;

        public CustomerService(FestispecContext db)
        {
            _db = db;
        }

        public List<Customer> GetAllCustomers()
        {
            return _db.Customers.ToList();
        }

        public async Task<Customer> CreateCustomerAsync(string name, int kvkNr, Address address, ContactDetails contactDetails)
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
            if (!customer.Validate() || !customer.Address.Validate() || !customer.ContactDetails.Validate())
                throw new InvalidDataException();

            _db.Customers.Add(customer);

            await SaveChangesAsync();

            return customer;
        }
        
        public async Task<Customer> GetCustomerAsync(int customerId)
        {
            Customer customer = await _db.Customers
                .Include(c => c.ContactPersons)
                .Include(c => c.Festivals)
                .FirstOrDefaultAsync(c => c.Id == customerId);

            if (customer == null)
                throw new EntityNotFoundException();

            return customer;
        }
        
        public Customer GetCustomer(int customerId)
        {
            Customer customer = _db.Customers
                .Include(c => c.ContactPersons)
                .Include(c => c.Festivals)
                .FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                throw new EntityNotFoundException();

            return customer;
        }

        public async Task<int> RemoveCustomerAsync(int customerId)
        {
            Customer customer = await GetCustomerAsync(customerId);

            if (customer.Festivals?.Count > 0)
                throw new CustomerHasFestivalsException();
            
            _db.ContactPersons.RemoveRange(customer.ContactPersons);
            _db.Customers.Remove(customer);

            return await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
