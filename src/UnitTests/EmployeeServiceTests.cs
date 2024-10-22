using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                "tester123",
                "testpassword",
                Role.Employee,
                new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Testing street",
                    HouseNumber = 1,
                    Suffix = "a",
                    City = "Test city",
                    Country = "Nederland",
                    Latitude = 12,
                    Longitude = 16
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
                "testerino",
                "testpassword",
                Role.Employee,
                new Address
                {
                    ZipCode = "1234AB",
                    StreetName = "Testing street",
                    HouseNumber = 1,
                    Suffix = "a",
                    City = "Test city",
                    Country = "Nederland",
                    Latitude = 15,
                    Longitude = 19
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
                "testertesttest",
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
            // Removed because it throws an InvalidAddressException
            //new object[]
            //{
            //    new FullName {First = "Test", Last = "Testerson"},
            //    "NL01RABO0123456789",
            //    "testeretta",
            //    "testpassword",
            //    Role.Employee,
            //    new Address
            //    {
            //        // te korte postcode
            //        ZipCode = "123",
            //        StreetName = "Testing street",
            //        HouseNumber = 1,
            //        Suffix = "a",
            //        City = "Test city",
            //        Country = "Nederland"
            //    },
            //    new ContactDetails
            //    {
            //        EmailAddress = "test@tester.com",
            //        PhoneNumber = "+316123456789"
            //    }
            //}
        };
        
        public static IEnumerable<object[]> ValidCertificates = new[]
        {
            new object[]
            {
                "Test certificate",
                new DateTime(2020, 10, 25),
                new DateTime(2025, 10, 25)
            },
            new object[]
            {
                "Testing certificate",
                new DateTime(2025, 12, 11),
                new DateTime(2030, 12, 11),
            }
        };
        
        public static IEnumerable<object[]> InvalidCertificates = new[]
        {
            new object[]
            {
                "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                new DateTime(2020, 10, 25),
                new DateTime(2025, 10, 25)
            },
            new object[]
            {
                "Testing certificate",
                new DateTime(2025, 12, 11),
                new DateTime(2020, 12, 11),
            }
        };

        public EmployeeServiceTests()
        {
            _dbMock = new Mock<FestispecContext>();
            _modelMocks = new ModelMocks();
            _dbMock.Setup(x => x.Employees).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Employees).Object);
            _dbMock.Setup(x => x.Accounts).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Accounts).Object);
            _dbMock.Setup(x => x.Certificates).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Certificates).Object);
            _dbMock.Setup(x => x.Addresses).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Addresses).Object);
            _dbMock.Setup(x => x.Festivals).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Festivals).Object);
            _dbMock.Setup(x => x.Customers).Returns(MockHelpers.CreateDbSetMock(_modelMocks.Customers).Object);

            _employeeService = new EmployeeService(_dbMock.Object,
                new Mock<AuthenticationService>(_dbMock.Object, new JsonSyncService<Account>(_dbMock.Object)).Object,
                new JsonSyncService<Employee>(_dbMock.Object),
                new AddressService(_dbMock.Object));
        }

        [Fact]
        public void GetAllEmployeesReturnsEmployeeList()
        {
            List<Employee> expected = _modelMocks.Employees.Where(e => e.Account.IsNonActive == null).ToList();
            
            List<Employee> actual = _employeeService.GetAllEmployees().ToList();
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void GetAllEmployeesIncludingNonActiveReturnsCompleteEmployeeList()
        {
            List<Employee> expected = _modelMocks.Employees;
            
            List<Employee> actual = _employeeService.GetAllEmployeesActiveAndNonActive().ToList();
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

             // once for the employee, once for the address
             _dbMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));

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
        [InlineData(3)]
        public async void RemoveEmployeeRemovesEmployee(int employeeId)
        {
            Assert.True(_employeeService.CanRemoveEmployee(_employeeService.GetEmployee(employeeId)));
            await _employeeService.RemoveEmployeeAsync(employeeId);
            
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.GetEmployeeAsync(employeeId));
            // once for the address, another for the employee
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
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

        [Theory]
        [InlineData(9999)]
        public void GetNonexistentCertificateThrowsException(int certificateId)
        {
            Assert.Throws<EntityNotFoundException>(() => _employeeService.GetCertificate(certificateId));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetCertificateReturnsCorrectCertificate(int certificateId)
        {
            Certificate expected = _dbMock.Object.Certificates.FirstOrDefault(c => c.Id == certificateId);
            Assert.Equal(expected, _employeeService.GetCertificate(certificateId));
        }

        [Theory]
        [MemberData(nameof(ValidCertificates))]
        public async void CreateCertificateAddsCertificate(string certificateTitle, DateTime certificationDate,
            DateTime expirationDate)
        {
            Certificate certificate = new Certificate
            {
                CertificateTitle = certificateTitle,
                CertificationDate = certificationDate,
                ExpirationDate = expirationDate,
                Employee = _dbMock.Object.Employees.FirstOrDefault()
            };

            Certificate createdCertificate = await _employeeService.CreateCertificateAsync(certificate);

            Assert.Equal(certificateTitle, createdCertificate.CertificateTitle);
            Assert.Equal(certificationDate, createdCertificate.CertificationDate);
            Assert.Equal(expirationDate, createdCertificate.ExpirationDate);

            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);

            Certificate retrievedCertificate = _employeeService.GetCertificate(createdCertificate.Id);
            Assert.Equal(createdCertificate, retrievedCertificate);
        }
        
        [Theory]
        [MemberData(nameof(InvalidCertificates))]
        public async void CreateCertificateWithInvalidDataThrowsException(string certificateTitle, DateTime certificationDate,
            DateTime expirationDate)
        {
            Certificate certificate = new Certificate
            {
                CertificateTitle = certificateTitle,
                CertificationDate = certificationDate,
                ExpirationDate = expirationDate,
                Employee = _dbMock.Object.Employees.FirstOrDefault()
            };
            
            await Assert.ThrowsAsync<InvalidDataException>(() => _employeeService.CreateCertificateAsync(certificate));
        }
        
        [Theory]
        [InlineData(1)]
        public async void RemoveCertificateRemovesCertificate(int certificateId)
        {
            await _employeeService.RemoveCertificateAsync(certificateId);
            
            Assert.Throws<EntityNotFoundException>(() => _employeeService.GetCertificate(certificateId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        
        [Theory]
        [InlineData(99999)]
        public async void RemoveNonexistentCertificateThrowsException(int certificateId)
        {
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _employeeService.RemoveCertificateAsync(certificateId));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task UpdateEmployeeAsyncUpdatesAddress(int employeeId)
        {
            Employee employee = await _employeeService.GetEmployeeAsync(employeeId);

            employee.Address.City = "Teststadje";
            employee.Address.Id = 99;
            await _employeeService.UpdateEmployee(employee);

            Assert.Equal("Teststadje", _dbMock.Object.Addresses.First(x => x.Id == 99).City);
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task UpdateEmployeeAsyncWithInvalidAddressThrowsException(int employeeId)
        {
            Employee employee = await _employeeService.GetEmployeeAsync(employeeId);

            employee.Address.City = new string('A', 205);
            await Assert.ThrowsAsync<InvalidAddressException>(async () => await _employeeService.UpdateEmployee(employee));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task UpdateEmployeeAsyncWithInvalidDataThrowsException(int employeeId)
        {
            Employee employee = await _employeeService.GetEmployeeAsync(employeeId);

            employee.Name = new FullName
            {
                First = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA",
                Middle = "AA",
                Last = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
            };
            await Assert.ThrowsAsync<InvalidDataException>(async () => await _employeeService.UpdateEmployee(employee));
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}