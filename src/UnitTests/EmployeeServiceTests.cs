using System.Collections.Generic;
using System.Linq;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using Xunit;

namespace Festispec.UnitTests
{
    public class EmployeeServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;
        private readonly IEmployeeService _employeeService;
        private ModelMocks _modelMocks;

        public EmployeeServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);
            _dbMock.Setup(x => x.Accounts).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Accounts).Object);

            _employeeService = new EmployeeService(_dbMock.Object, new AuthenticationService(_dbMock.Object));
        }

        [Fact]
        public void GetAllEmployeesReturnsEmployeeList()
        {
            List<Employee> expected = _modelMocks.Employees;
            
            List<Employee> actual = _employeeService.GetAllEmployees();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetEmployeeReturnsCorrectEmployee(int customerId)
        {
            Employee expected = _dbMock.Object.Employees.FirstOrDefault(c => c.Id == customerId);
            Assert.Equal(expected, _employeeService.GetEmployee(customerId));
        }
        
        [Theory]
        [InlineData(9999)]
        public void GetNonexistentEmployeeThrowsException(int customerId)
        {
            Assert.Throws<EntityNotFoundException>(() => _employeeService.GetEmployee(customerId));
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetEmployeeAsyncReturnsCorrectEmployee(int customerId)
        {
            Employee expected = _dbMock.Object.Employees.FirstOrDefault(c => c.Id == customerId);
            Assert.Equal(expected, await _employeeService.GetEmployeeAsync(customerId));
        }
        
        [Theory]
        [InlineData(9999)]
        public async void GetNonexistentEmployeeAsyncThrowsException(int customerId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.GetEmployeeAsync(customerId));
        }

        /*[Theory]
        [InlineData("PinkPop", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        [InlineData("Q-DANCE", 34212891, "1014AS", "Isolatorweg", 36, "Amsterdam", "Nederland", "+31204877300", "info@q-dance.com")]
        public async void CreateEmployeeAddsEmployee(string name, int kvkNr, string zipCode, string street, int houseNumber, string city, string country, string phoneNumber, string emailAddress)
        {
            var address = new Address
            {
                City = city, Country = country, HouseNumber = houseNumber, StreetName = street, ZipCode = zipCode
            };

            var contactDetails = new ContactDetails
            {
                EmailAddress = emailAddress, PhoneNumber = phoneNumber
            };
            
            Employee createdEmployee = await _employeeService.CreateEmployeeAsync(name, kvkNr, address, contactDetails);

            Assert.Equal(name, createdEmployee.EmployeeName);
            Assert.Equal(createdEmployee.KvkNr, kvkNr);
            Assert.Equal(createdEmployee.Address, address);
            Assert.Equal(createdEmployee.ContactDetails, contactDetails);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            Employee customer = await _employeeService.GetEmployeeAsync(createdEmployee.Id);
            Assert.Equal(createdEmployee, customer);
        }*/
        
        // [Theory]
        // [InlineData("PinkPopDitIsEenHeelLangeNaamDieBovenDe20KaraktersUitKomt", 12345678, "1013 GM", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        // [InlineData("PinkPop", 12345678, "1013 AAAAAAAAAAAAAAAAAAB", "Amsterweg", 23, "Utrecht", "Nederland", "31695734859", "psmulde@pinkpop.nl")]
        // public async void CreateEmployeeWithInvalidDataThrowsException(string name, int kvkNr, string zipCode, string street,
        //     int houseNumber, string city, string country, string phoneNumber, string emailAddress)
        // {
        //     var address = new Address
        //     {
        //         City = city, Country = country, HouseNumber = houseNumber, StreetName = street, ZipCode = zipCode
        //     };
        //
        //     var contactDetails = new ContactDetails
        //     {
        //         EmailAddress = emailAddress, PhoneNumber = phoneNumber
        //     };
        //     
        //     await Assert.ThrowsAsync<InvalidDataException>(() => _employeeService.CreateEmployeeAsync(name, kvkNr, address, contactDetails));
        // }

        [Theory]
        [InlineData(1)]
        public async void RemoveEmployeeRemovesEmployee(int customerId)
        {
            await _employeeService.RemoveEmployeeAsync(customerId);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.GetEmployeeAsync(customerId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(99999)]
        public async void RemoveNonexistentEmployeeThrowsException(int customerId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.RemoveEmployeeAsync(customerId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        // [Theory]
        // [InlineData(2)]
        // public async void RemoveEmployeeWithFestivalsThrowsException(int customerId)
        // {
        //     await Assert.ThrowsAsync<EmployeeHasFestivalsException>(() => _employeeService.RemoveEmployeeAsync(customerId));
        //     _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        // }
    }
}