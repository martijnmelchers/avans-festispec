
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using System;
using System.Linq;
using System.Threading.Tasks;
using Festispec.Models.EntityMapping;
using System.ComponentModel.DataAnnotations;

namespace Festispec.DomainServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly FestispecContext _db;

        public AuthenticationService(FestispecContext db) {
            _db = db;
        }
        public async Task<Account> Register(string username, string password, Role requiredRole)
        {
            var existing = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (existing != null)
                throw new AccountExistsException();

            var account = new Account()
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Role = requiredRole
            };

            if (!account.Validate(password))
                throw new InvalidDataException();

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            return account.ToSafeAccount();
        }

        public Account Login(string username, string password, Role requiredRole)
        {
            var account = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                throw new AuthenticationException("Username or password are incorrect");

            if (account.Role != requiredRole)
                throw new NotAuthorizedException();
            
            return account.ToSafeAccount();
        }

        public async Task ChangePassword(string username, string password, string newPassword)
        {
            var account = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                throw new AuthenticationException("Username or password are incorrect");

            account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _db.SaveChangesAsync();
        }
    }
}
