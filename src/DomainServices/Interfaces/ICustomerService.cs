﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Task<Customer> CreateCustomer(string name, int kvkNr, Address address, ContactDetails contactDetails);
        Customer GetCustomer(int customerId);
        Task RemoveCustomer(int customerId);
        Task SaveChanges();
    }
}