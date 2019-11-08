using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Account> Register(string username, string password);
        Account Login(string username, string password);
        Task ChangePassword(string username, string password, string newPassword);
    }
}
