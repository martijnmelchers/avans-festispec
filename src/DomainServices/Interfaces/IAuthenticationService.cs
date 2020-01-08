using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IAuthenticationService
    {
        Account AssembleAccount(string username, string password, Role requiredRole);
        Account Login(string username, string password, Role requiredRole);
        Task ChangePassword(string username, string password, string newPassword);
    }
}