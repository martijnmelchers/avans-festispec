using System.Collections.Generic;
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
        private ModelMocks _modelMocks;

        public CustomerServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();
            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Customers).Object);
            _dbMock.Setup(x => x.ContactPersons).Returns(MockHelpers.CreateDbSetMock(_modelMocks.ContactPersons).Object);

            _customerService = new CustomerService(_dbMock.Object, new JsonSyncService<Customer>(_dbMock.Object));
        }

        [Fact]
        public void GetAllCustomersReturnsCustomerList()
        {
            List<Customer> expected = _modelMocks.Customers;
            
            List<Customer> actual = _customerService.GetAllCustomers().ToList();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetCustomerReturnsCorrectCustomer(int customerId)
        {
            Customer expected = _dbMock.Object.Customers.FirstOrDefault(c => c.Id == customerId);
            Assert.Equal(expected, _customerService.GetCustomer(customerId));
        }
        
        [Theory]
        [InlineData(9999)]
        public void GetNonexistentCustomerThrowsException(int customerId)
        {
            Assert.Throws<EntityNotFoundException>(() => _customerService.GetCustomer(customerId));
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetCustomerAsyncReturnsCorrectCustomer(int customerId)
        {
            Customer expected = _dbMock.Object.Customers.FirstOrDefault(c => c.Id == customerId);
            Assert.Equal(expected, await _customerService.GetCustomerAsync(customerId));
        }
        
        [Theory]
        [InlineData(9999)]
        public async void GetNonexistentCustomerAsyncThrowsException(int customerId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.GetCustomerAsync(customerId));
        }

        [Theory]
        [InlineData("PinkPop", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        [InlineData("Q-DANCE", 34212891, "1014AS", "Isolatorweg", 36, "Amsterdam", "Nederland", "+31204877300", "info@q-dance.com")]
        public async void CreateCustomerAddsCustomer(string name, int kvkNr, string zipCode, string street, int houseNumber, string city, string country, string phoneNumber, string emailAddress)
        {
            var address = new Address
            {
                City = city, Country = country, HouseNumber = houseNumber, StreetName = street, ZipCode = zipCode
            };

            var contactDetails = new ContactDetails
            {
                EmailAddress = emailAddress, PhoneNumber = phoneNumber
            };
            
            Customer createdCustomer = await _customerService.CreateCustomerAsync(name, kvkNr, address, contactDetails);

            Assert.Equal(name, createdCustomer.CustomerName);
            Assert.Equal(createdCustomer.KvkNr, kvkNr);
            Assert.Equal(createdCustomer.Address, address);
            Assert.Equal(createdCustomer.ContactDetails, contactDetails);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            Customer customer = await _customerService.GetCustomerAsync(createdCustomer.Id);
            Assert.Equal(createdCustomer, customer);
        }
        
        [Theory]
        [InlineData("PinkPopDitIsEenHeelLangeNaamDieBovenDe20KaraktersUitKomt", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        [InlineData("PinkPop", 12345678, "1013 AAAAAAAAAAAAAAAAAAB", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        public async void CreateCustomerWithInvalidDataThrowsException(string name, int kvkNr, string zipCode, string street,
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
        public async void RemoveCustomerRemovesCustomer(int customerId)
        {
            await _customerService.RemoveCustomerAsync(customerId);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.GetCustomerAsync(customerId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(99999)]
        public async void RemoveNonexistentCustomerThrowsException(int customerId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.RemoveCustomerAsync(customerId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(2)]
        public async void RemoveCustomerWithFestivalsThrowsException(int customerId)
        {
            await Assert.ThrowsAsync<CustomerHasFestivalsException>(() => _customerService.RemoveCustomerAsync(customerId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}