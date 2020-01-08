﻿using System.Collections.Generic;
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
        private readonly IAddressService _addressService;
        private readonly FestispecContext _db;

        public CustomerService(FestispecContext db, IAddressService addressService)
        {
            _db = db;
            _addressService = addressService;
        }

        public List<Customer> GetAllCustomers()
        {
            return _db.Customers.Include(c => c.Address).ToList();
        }

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
            Customer customer = await _db.Customers
                .Include(c => c.ContactPersons)
                .Include(c => c.Festivals)
                .Include(c => c.Address)
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
                .Include(c => c.Address)
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
            await _addressService.RemoveAddress(customer.Address);

            return await SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            if (!customer.Validate() || !customer.ContactDetails.Validate())
                throw new InvalidDataException();

            customer.Address = await _addressService.SaveAddress(customer.Address);

            await SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}