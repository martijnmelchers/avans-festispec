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
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();

            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Customers).Object);

            _customerService = new CustomerService(_dbMock.Object);
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
            
            var customer = await _customerService.CreateCustomer(name, kvkNr, address, contactDetails);

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
            
            await Assert.ThrowsAsync<InvalidDataException>(() => _customerService.CreateCustomer(name, kvkNr, address, contactDetails));
        }
    }
}