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
        private readonly ModelMocks _modelMocks;

        public static IEnumerable<object[]> ValidEmployees => new[]
        {
            new object[]
            {
                new FullName {First = "Test", Last = "Testerson"},
                "NL01RABO0123456789",
                "tester",
                "testpassword",
                Role.Employee,
                new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Testing street",
                    HouseNumber = 1,
                    Suffix = "a",
                    City = "Test city",
                    Country = "Nederland"
                },
                new ContactDetails
                {
                    EmailAddress = "test@tester.com",
                    PhoneNumber = "+316123456789"
                }
            }
        };

        public static IEnumerable<object[]> InvalidEmployees => new[]
        {
            new object[]
            {
                new FullName {First = "Test", Last = "Testerson"},
                "EMPLOYEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEET",
                "tester",
                "testpassword",
                Role.Employee,
                new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Testing street",
                    HouseNumber = 1,
                    Suffix = "a",
                    City = "Test city",
                    Country = "Nederland"
                },
                new ContactDetails
                {
                    EmailAddress = "test@tester.com",
                    PhoneNumber = "+316123456789"
                }
            },
            new object[]
            {
                new FullName {First = "Veel te lange naam met spaties enzo, totaal niet wat geaccepteerd moet worden", Last = "Testerson"},
                "NL01RABO0123456789",
                "tester",
                "testpassword",
                Role.Employee,
                new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Testing street",
                    HouseNumber = 1,
                    Suffix = "a",
                    City = "Test city",
                    Country = "Nederland"
                },
                new ContactDetails
                {
                    EmailAddress = "test@tester.com",
                    PhoneNumber = "+316123456789"
                }
            },
            new object[]
            {
                new FullName {First = "Test", Last = "Testerson"},
                "NL01RABO0123456789",
                "tester",
                "testpassword",
                Role.Employee,
                new Address
                {
                    // te korte postcode
                    ZipCode = "123",
                    StreetName = "Testing street",
                    HouseNumber = 1,
                    Suffix = "a",
                    City = "Test city",
                    Country = "Nederland"
                },
                new ContactDetails
                {
                    EmailAddress = "test@tester.com",
                    PhoneNumber = "+316123456789"
                }
            }
        };

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
        public void GetEmployeeReturnsCorrectEmployee(int employeeId)
        {
            Employee expected = _dbMock.Object.Employees.FirstOrDefault(c => c.Id == employeeId);
            Assert.Equal(expected, _employeeService.GetEmployee(employeeId));
        }
        
        [Theory]
        [InlineData(9999)]
        public void GetNonexistentEmployeeThrowsException(int employeeId)
        {
            Assert.Throws<EntityNotFoundException>(() => _employeeService.GetEmployee(employeeId));
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetEmployeeAsyncReturnsCorrectEmployee(int employeeId)
        {
            Employee expected = _dbMock.Object.Employees.FirstOrDefault(c => c.Id == employeeId);
            Assert.Equal(expected, await _employeeService.GetEmployeeAsync(employeeId));
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAccountForEmployeeAsyncReturnsCorrectAccount(int employeeId)
        {
            Account expected = _dbMock.Object.Employees.FirstOrDefault(c => c.Id == employeeId).Account;
            Assert.Equal(expected, _employeeService.GetAccountForEmployee(employeeId));
        }
        
        
        [Theory]
        [InlineData(9999)]
        public async void GetNonexistentEmployeeAsyncThrowsException(int employeeId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.GetEmployeeAsync(employeeId));
        }
        
        [Theory]
        [InlineData(9999)]
        public void GetAccountForNonexistentEmployeeAsyncThrowsException(int employeeId)
        {
            Assert.Throws<EntityNotFoundException>(() => _employeeService.GetAccountForEmployee(employeeId));
        }

         [Theory]
         [MemberData(nameof(ValidEmployees))]
         public async void CreateEmployeeAddsEmployee(FullName fullName, string iban, string username, string password, Role role, Address address, ContactDetails contactDetails)
         {
             Employee createdEmployee = await _employeeService.CreateEmployeeAsync(fullName, iban, username, password, role, address, contactDetails);

             Assert.Equal(fullName, createdEmployee.Name);
             Assert.Equal(createdEmployee.Iban, iban);
             Assert.Equal(createdEmployee.Address, address);
             Assert.Equal(createdEmployee.ContactDetails, contactDetails);
             Assert.Equal(role, createdEmployee.Account.Role);
             Assert.Equal(username, createdEmployee.Account.Username);
             Assert.True(BCrypt.Net.BCrypt.Verify(password, createdEmployee.Account.Password));

             _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);

             Employee customer = await _employeeService.GetEmployeeAsync(createdEmployee.Id);
             Assert.Equal(createdEmployee, customer);
         }
         
         [Theory]
         [MemberData(nameof(InvalidEmployees))]
         public async void CreateEmployeeWithInvalidDataThrowsException(FullName fullName, string iban, string username, string password, Role role, Address address, ContactDetails contactDetails)
         {
             await Assert.ThrowsAsync<InvalidDataException>(() => _employeeService.CreateEmployeeAsync(fullName, iban, username, password, role, address, contactDetails));
         }

         [Theory]
        [InlineData(1)]
        public async void RemoveEmployeeRemovesEmployee(int employeeId)
        {
            Assert.True(_employeeService.CanRemoveEmployee(_employeeService.GetEmployee(employeeId)));
            await _employeeService.RemoveEmployeeAsync(employeeId);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.GetEmployeeAsync(employeeId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(99999)]
        public async void RemoveNonexistentEmployeeThrowsException(int employeeId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.RemoveEmployeeAsync(employeeId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(2)]
        public async void RemoveEmployeeWithFestivalsThrowsException(int employeeId)
        {
            Assert.False(_employeeService.CanRemoveEmployee(_employeeService.GetEmployee(employeeId)));
            await Assert.ThrowsAsync<EmployeeHasPlannedEventsException>(() => _employeeService.RemoveEmployeeAsync(employeeId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}