using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            return _db.Customers
                .Include(c => c.ContactPersons)
                .Include(c => c.ContactPersons.Select(cp => cp.Notes))
                .Include(c => c.Festivals)
                .ToList();
        }

        public async Task<Customer> CreateCustomer(string name, int kvkNr, Address address, ContactDetails contactDetails)
        {
            var customer = new Customer()
            {
                CustomerName = name,
                KvkNr = kvkNr,
                Address = address,
                ContactDetails = contactDetails
            };

            if (!customer.Validate())
                throw new InvalidDataException();

            _db.Customers.Add(customer);

            await _db.SaveChangesAsync();

            return customer;
        }

        public Customer GetCustomer(int customerId)
        {
            Customer customer = _db.Customers
                .Include(c => c.ContactPersons)
                .Include(c => c.ContactPersons.Select(cp => cp.Notes))
                .Include(c => c.Festivals)
                .FirstOrDefault(c => c.Id == customerId);

            if (customer == null)
                // TODO: Change to EntityNotFoundException.
                throw new InvalidOperationException();

            return customer;
        }

        public async Task RemoveCustomer(int customerId)
        {
            Customer customer = GetCustomer(customerId);

            if (customer.Festivals.Count > 0)
                throw new CustomerHasFestivalsException();

            _db.ContactPersons.RemoveRange(customer.ContactPersons);
            _db.Customers.Remove(customer);

            await _db.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _db.SaveChangesAsync();
        }
    }
}
