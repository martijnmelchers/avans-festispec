using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;

namespace Festispec.DomainServices.Services
{
    public class OfflineAuthenticationService : IAuthenticationService
    {
        private readonly ISyncService<Account> _syncService;

        public OfflineAuthenticationService(ISyncService<Account> syncService)
        {
            _syncService = syncService;
        }
        
        public Account AssembleAccount(string username, string password, Role requiredRole)
        {
            throw new System.InvalidOperationException();
        }

        public Account Login(string username, string password, Role requiredRole)
        {
            Account account = _syncService.GetAll().FirstOrDefault(x => x.Username == username);

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.Password))
                throw new AuthenticationException("Username or password are incorrect");

            if (account.Role != requiredRole)
                throw new NotAuthorizedException();
            
            return account.ToSafeAccount();
        }

        public Task ChangePassword(string username, string password, string newPassword)
        {
            throw new System.InvalidOperationException();
        }

        public void Sync()
        {
        }
    }
}