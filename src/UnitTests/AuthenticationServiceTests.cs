using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Festispec.UnitTests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<FestispecContext> _dbMock;

        private readonly IAuthenticationService _authenticationService;

        public AuthenticationServiceTests()
        {
            // Setup database mocks
            _dbMock = new Mock<FestispecContext>();

            // Setup add mock
            _dbMock.Setup(x => x.Accounts.Add(It.IsAny<Account>())).Returns((Account u) => u);


            // Setup accounts
            var testAccounts = new List<Account>()
            {
                new Account()
                {
                    Username = "JohnDoe",
                    Password = BCrypt.Net.BCrypt.HashPassword("Password123")
                },
                new Account()
                {
                    Username = "EricKuipers",
                    Password = BCrypt.Net.BCrypt.HashPassword("HeelLangWachtwoord")
                }
            };

            // Mock accounts
            _dbMock.Setup(x => x.Accounts).Returns(MockHelpers.CreateDbSetMock(testAccounts).Object);

            // Create AuthenticationService
            _authenticationService = new AuthenticationService(_dbMock.Object);
        }


        [Fact]
        public async void CanRegister()
        {
            // Expected username & password
            var expectedUsername = "JohnDoe";
            var expectedPassword = "Password123";

            // Register user
            var account = await _authenticationService.Register(expectedUsername, expectedPassword);

            // Check if values have been properly assigned and password has been encryped
            Assert.Equal(expectedUsername, account.Username);
            Assert.Null(account.Password);

            // Verify if the SaveChangesAsync has been called
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("JohnDoe", "Password123", true)]
        [InlineData("JohnDoe", "Password1234", false)]
        [InlineData("EricKuipers", "HeelLangWachtwoord", true)]
        [InlineData("EricKuipers", "HeelLangWachtwoort", false)]
        [InlineData("NotExistantUser", "NotExistantPassword", false)]
        public void CanLogin(string username, string password, bool shouldBeAbleToLogin)
        {
            if (!shouldBeAbleToLogin)
            {
                // Should throw error if the password & username do not match or if the user does not exist
                Assert.Throws<AuthenticationException>(() => _authenticationService.Login(username, password));
            }
            else
            {
                var account = _authenticationService.Login(username, password);

                // Username should match & sensitive information should be removed
                Assert.Equal(username, account.Username);
                Assert.Null(account.Password);
            }
        }

        [Theory]
        [InlineData("JohnDoe", "Password123", "Password1234", true)]
        [InlineData("JohnDoe", "Password1234", "Password1234", false)]
        public void CanChangePassword(string username, string password, string newPassword, bool shouldBeAbleToChangePassword)
        {
            if(!shouldBeAbleToChangePassword)
            {
                // Should throw error if the password & username do not match or if the user does not exist
                Assert.ThrowsAsync<AuthenticationException>(() => _authenticationService.ChangePassword(username, password, newPassword));
            }
            else
            {
                _authenticationService.ChangePassword(username, password, newPassword);

                _dbMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            }
        }
    }
}
