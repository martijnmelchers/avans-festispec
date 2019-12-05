using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Festispec.Models.EntityMapping;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using Xunit;

namespace Festispec.UnitTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly ICustomerService _customerService;
        public CustomerServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Customers).Object);
            _dbMock.Setup(x => x.ContactPersons).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().ContactPersons).Object);

            _customerService = new CustomerService(_dbMock.Object);
        }

        [Theory]
        [InlineData(1)]
        public void GetCustomer(int customerId)
        {
            Customer expected = _dbMock.Object.Customers.FirstOrDefault(c => c.Id == customerId);
            Assert.Equal(expected, _customerService.GetCustomer(customerId));
        }
        
        [Theory]
        [InlineData(9999)]
        public void GetNonexistentCustomer(int customerId)
        {
            Assert.Throws<EntityNotFoundException>(() => _customerService.GetCustomer(customerId));
        }
        
        [Theory]
        [InlineData(1)]
        public async void GetCustomerAsync(int customerId)
        {
            Customer expected = _dbMock.Object.Customers.FirstOrDefault(c => c.Id == customerId);
            Assert.Equal(expected, await _customerService.GetCustomerAsync(customerId));
        }
        
        [Theory]
        [InlineData(9999)]
        public async void GetNonexistentCustomerAsync(int customerId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.GetCustomerAsync(customerId));
        }

        [Theory]
        [InlineData("PinkPop", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        [InlineData("Q-DANCE", 34212891, "1014AS", "Isolatorweg", 36, "Amsterdam", "Nederland", "+31204877300", "info@q-dance.com")]
        public async void CreateCustomer(string name, int kvkNr, string zipCode, string street, int houseNumber, string city, string country, string phoneNumber, string emailAddress)
        {
            var address = new Address
            {
                City = city, Country = country, HouseNumber = houseNumber, StreetName = street, ZipCode = zipCode
            };

            var contactDetails = new ContactDetails
            {
                EmailAddress = emailAddress, PhoneNumber = phoneNumber
            };
            
            Customer customer = await _customerService.CreateCustomerAsync(name, kvkNr, address, contactDetails);

            Assert.Equal(name, customer.CustomerName);
            Assert.Equal(customer.KvkNr, kvkNr);
            Assert.Equal(customer.Address, address);
            Assert.Equal(customer.ContactDetails, contactDetails);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        
        [Theory]
        [InlineData("PinkPopDitIsEenHeelLangeNaamDieBovenDe20KaraktersUitKomt", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        [InlineData("PinkPop", 12345678, "1013 AAAAAAAAAAAAAAAAAAB", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        public async void CreateCustomerInvalidData(string name, int kvkNr, string zipCode, string street,
            int houseNumber, string city, string country, string phoneNumber, string emailAddress)
        {
            var address = new Address
            {
                City = city, Country = country, HouseNumber = houseNumber, StreetName = street, ZipCode = zipCode
            };

            var contactDetails = new ContactDetails
            {
                EmailAddress = emailAddress, PhoneNumber = phoneNumber
            };
            
            await Assert.ThrowsAsync<InvalidDataException>(() => _customerService.CreateCustomerAsync(name, kvkNr, address, contactDetails));
        }

        [Theory]
        [InlineData(1)]
        public async void RemoveCustomer(int customerId)
        {
            await _customerService.RemoveCustomerAsync(customerId);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.GetCustomerAsync(customerId));
        }

        [Theory]
        [InlineData(99999)]
        public async void RemoveNonexistentCustomer(int customerId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.RemoveCustomerAsync(customerId));
        }

        [Theory]
        [InlineData(2)]
        public async void RemoveCustomerWithFestivals(int customerId)
        {
            await Assert.ThrowsAsync<CustomerHasFestivalsException>(() => _customerService.RemoveCustomerAsync(customerId));
        }
    }
}