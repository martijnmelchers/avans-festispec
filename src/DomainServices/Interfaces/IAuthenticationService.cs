using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IAuthenticationService
    {
        Account AssembleAccount(string username, string password, Role requiredRole);
        Account Login(string username, string password, Role requiredRole);
        Task ChangePassword(string username, string password, string newPassword);
    }
}
