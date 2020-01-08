using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly FestispecContext _db;

        public AuthenticationService(FestispecContext db)
        {
            _db = db;
        }


        public Account AssembleAccount(string username, string password, Role requiredRole)
        {
            Account existing = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (existing != null)
                throw new EntityExistsException();

            var account = new Account
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password),
                Role = requiredRole
            };

            if (!account.Validate(password))
                throw new InvalidDataException();

            return account;
        }

        public Account Login(string username, string password, Role requiredRole)
        {
            Account account = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                throw new AuthenticationException("Username or password are incorrect");

            if (account.Role != requiredRole)
                throw new NotAuthorizedException();

            if (account.IsNonActive != null)
                throw new NotAuthorizedException();

            return account.ToSafeAccount();
        }

        public async Task ChangePassword(string username, string password, string newPassword)
        {
            Account account = _db.Accounts.FirstOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                throw new AuthenticationException("Username or password are incorrect");

            account.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _db.SaveChangesAsync();
        }
    }
}