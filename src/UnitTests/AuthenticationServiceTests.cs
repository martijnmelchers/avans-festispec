using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.UnitTests.Helpers;
using Moq;
using System.Collections.Generic;
using Festispec.Models.EntityMapping;
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

            // Mock accounts
            _dbMock.Setup(x => x.Accounts).Returns(MockHelpers.CreateDbSetMock(new ModelMocks().Accounts).Object);

            // Create AuthenticationService
            _authenticationService = new AuthenticationService(_dbMock.Object, new JsonSyncService<Account>(_dbMock.Object));
        }

        #region Registration Tests

        [Theory]
        [InlineData("Username1", "Password1234", Role.Employee)]
        [InlineData("Username2", "Password1234", Role.Inspector)]
        public void ValidRegisterDataReturnsSafeAccount(string username, string password, Role requiredRole)
        {
            // Register user
            var account = _authenticationService.AssembleAccount(username, password, requiredRole);

            // Verify that the SaveChangesAsync method has not been called
            _dbMock.Verify(x => x.SaveChangesAsync(), Times.Never);

            // Check if values have been properly assigned and password has been encrypted
            Assert.Equal(username, account.Username);
            Assert.Equal(requiredRole, account.Role);
            Assert.True(BCrypt.Net.BCrypt.Verify(password, account.Password));
        }

        [Theory]
        [InlineData("JohnDoe", "Password123", Role.Employee)]
        [InlineData("EricKuipers", "HeelLangWachtwoord", Role.Inspector)]
        public void SameUsernameShouldThrowError(string username, string password, Role requiredRole)
        {
            Assert.Throws<EntityExistsException>(() => _authenticationService.AssembleAccount(username, password, requiredRole));
        }

        [Theory]
        [InlineData("123", "123", Role.Employee)]
        [InlineData("123", "123", Role.Inspector)]
        public void InvalidDataShouldThrowError(string username, string password, Role requiredRole)
        {
            Assert.Throws<InvalidDataException>(() => _authenticationService.AssembleAccount(username, password, requiredRole));
        }

        #endregion

        #region Login Tests
        [Theory]
        [InlineData("JohnDoe", "Password123", Role.Employee)]
        [InlineData("EricKuipers", "HeelLangWachtwoord", Role.Inspector)]
        public void ValidLoginInfoReturnsSafeAccount(string username, string password, Role requiredRole)
        {
            var account = _authenticationService.Login(username, password, requiredRole);

            // Username should match & sensitive information should be removed
            Assert.Equal(username, account.Username);
            Assert.Equal(requiredRole, account.Role);
            Assert.Null(account.Password);
        }

        [Theory]
        [InlineData("JohnDoe", "Password1234", Role.Employee)]
        [InlineData("EricKuipers", "HeelLangWachtwoort", Role.Inspector)]
        [InlineData("NotExistantUser", "NotExistantPassword", Role.Employee)]
        public void InvalidLoginInfoThrowsError(string username, string password, Role requiredRole)
        {
            Assert.Throws<AuthenticationException>(() => _authenticationService.Login(username, password, requiredRole));
        }


        [Theory]
        [InlineData("JohnDoe", "Password123", Role.Inspector)]
        [InlineData("EricKuipers", "HeelLangWachtwoord", Role.Employee)]
        public void IncorrectRoleThrowsError(string username, string password, Role requiredRole)
        {
            Assert.Throws<NotAuthorizedException>(() => _authenticationService.Login(username, password, requiredRole));
        }

        #endregion

        #region Password Change Tests
        [Theory]
        [InlineData("JohnDoe", "Password123", "Password1234", true)]
        [InlineData("JohnDoe", "Password1234", "Password1234", false)]
        public void CanChangePassword(string username, string password, string newPassword, bool shouldBeAbleToChangePassword)
        {
            if (!shouldBeAbleToChangePassword)
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

        #endregion
    }
}
