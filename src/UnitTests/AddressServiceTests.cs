using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using Xunit;

namespace Festispec.UnitTests
{
    public class AddressServiceTests
    {
        public AddressServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();
            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Customers).Object);
            _dbMock.Setup(x => x.Addresses).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Addresses).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Festivals).Object);
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);

            _addressService = new AddressService(_dbMock.Object);
        }

        private readonly Mock<FestispecContext> _dbMock;
        private readonly ModelMocks _modelMocks;
        private readonly AddressService _addressService;

        public static IEnumerable<object[]> ValidAddresses => new[]
        {
            new object[]
            {
                new Address
                {
                    ZipCode = "1072JL",
                    StreetName = "Lutmastraat",
                    HouseNumber = 14,
                    City = "Amsterdam",
                    Country = "Nederland",
                    Latitude = 52.3504f,
                    Longitude = 4.89271f
                }
            }
        };

        public static IEnumerable<object[]> InvalidAddresses => new[]
        {
            new object[]
            {
                new Address
                {
                    ZipCode = "VeelTeLangePostcode",
                    StreetName = "Lutmastraat",
                    HouseNumber = 14,
                    City = "Amsterdam",
                    Country = "Nederland",
                    Latitude = 52.3504f,
                    Longitude = 4.89271f
                }
            },
            new object[]
            {
                new Address
                {
                    ZipCode = "1072JL",
                    StreetName = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                    HouseNumber = 14,
                    City = "Amsterdam",
                    Country = "Nederland",
                    Latitude = 52.3504f,
                    Longitude = 4.89271f
                }
            },
            new object[]
            {
                new Address
                {
                    ZipCode = "1072JL",
                    StreetName = "Lutmastraat",
                    HouseNumber = 14,
                    City = "Amsterdam",
                    // mist land
                    Latitude = 52.3504f,
                    Longitude = 4.89271f
                }
            }
        };

        public static IEnumerable<object[]> UnusedAddresses => new[]
        {
            new object[]
            {
                new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Teststraat",
                    HouseNumber = 3,
                    City = "Amsterdam",
                    Country = "Nederland",
                    Latitude = 3f,
                    Longitude = 5f
                }
            }
        };

        [Theory]
        [MemberData(nameof(ValidAddresses))]
        public async Task SaveAddressShouldSaveAddress(Address address)
        {
            Address saved = await _addressService.SaveAddress(address);

            Assert.Equal(address, saved);
            Assert.True(_dbMock.Object.Addresses.Contains(address));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ValidAddresses))]
        public async Task SaveAddressShouldReturnExistingAddress(Address address)
        {
            // save it so it exists in the mock database.
            await _addressService.SaveAddress(address);

            Address existing = await _addressService.SaveAddress(address);

            Assert.Equal(address, existing);
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(InvalidAddresses))]
        public async Task SaveAddressWithInvalidDataShouldThrowException(Address address)
        {
            Assert.False(address.Validate());

            await Assert.ThrowsAsync<InvalidAddressException>(async () => await _addressService.SaveAddress(address));
        }

        [Theory]
        [MemberData(nameof(UnusedAddresses))]
        public async Task RemoveAddressWithoutUsagesShouldRemove(Address address)
        {
            await _addressService.SaveAddress(address);
            await _addressService.RemoveAddress(address);

            _dbMock.Verify(x => x.Addresses.Remove(address), Times.Once);
        }

        [Fact]
        public async Task RemoveAddressWithUsagesShouldNotRemove()
        {
            Address address = _modelMocks.Addresses.First();

            await _addressService.RemoveAddress(address);

            Assert.Contains(address, _dbMock.Object.Addresses.ToList());
            _dbMock.Verify(x => x.Addresses.Remove(address), Times.Never);
        }
    }
}