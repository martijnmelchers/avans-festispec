
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly FestispecContext _db;

        public AuthenticationService(FestispecContext db) {
            _db = db;
        }
        public async Task<Account> Register(string username, string password)
        {
            var account = new Account()
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            return account.ToSafeAccount();
        }

        public Account Login(string username, string password)
        {
            var account = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                throw new AuthenticationException("Username or password are incorrect");

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
