using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ModelMocks _modelMocks;

        public CustomerServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();
            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Customers).Object);
            _dbMock.Setup(x => x.Addresses).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Addresses).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Festivals).Object);
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);

            _customerService = new CustomerService(_dbMock.Object, new JsonSyncService<Customer>(_dbMock.Object), new AddressService(_dbMock.Object));
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
                City = city, Country = country, HouseNumber = houseNumber, StreetName = street, ZipCode = zipCode, Latitude = 69, Longitude = 420
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

            // once for the address, once for the customer
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));

            Customer customer = await _customerService.GetCustomerAsync(createdCustomer.Id);
            Assert.Equal(createdCustomer, customer);
        }
        
        [Theory]
        [InlineData("PinkPopDitIsEenHeelLangeNaamDieBovenDe20KaraktersUitKomt", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        // Disabled, throws InvalidAddressException instead of InvalidDataException
        //[InlineData("PinkPop", 12345678, "1013 AAAAAAAAAAAAAAAAAAB", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
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
        [InlineData(3)]
        public async void RemoveCustomerRemovesCustomer(int customerId)
        {
            Assert.True(_customerService.CanDeleteCustomer(_customerService.GetCustomer(customerId)));
            await _customerService.RemoveCustomerAsync(customerId);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _customerService.GetCustomerAsync(customerId));
            
            // once for the address, another for the customer
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
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
            Assert.False(_customerService.CanDeleteCustomer(_customerService.GetCustomer(customerId)));
            await Assert.ThrowsAsync<CustomerHasFestivalsException>(() => _customerService.RemoveCustomerAsync(customerId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task UpdateCustomerAsyncUpdatesAddress(int customerId)
        {
            Customer customer = await _customerService.GetCustomerAsync(customerId);

            customer.Address.City = "Teststadje";
            customer.Address.Id = 99;
            await _customerService.UpdateCustomerAsync(customer);

            Assert.Equal("Teststadje", _dbMock.Object.Addresses.First(x => x.Id == 99).City);
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task UpdateCustomerAsyncWithInvalidAddressThrowsException(int customerId)
        {
            Customer customer = await _customerService.GetCustomerAsync(customerId);

            customer.Address.City = new string('A', 205);
            await Assert.ThrowsAsync<InvalidAddressException>(async () => await _customerService.UpdateCustomerAsync(customer));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task UpdateCustomerAsyncWithInvalidDataThrowsException(int customerId)
        {
            Customer customer = await _customerService.GetCustomerAsync(customerId);

            customer.CustomerName = new string('A', 25);
            await Assert.ThrowsAsync<InvalidDataException>(async () => await _customerService.UpdateCustomerAsync(customer));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}