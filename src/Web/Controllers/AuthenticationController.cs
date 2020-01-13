using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models;
using Festispec.Models.Exception;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ActionResult Login()
        {
            if (Request.Cookies["CurrentUser"] != null && Request.Cookies["CurrentUserId"] != null) { }
            {
                Response.Cookies.Delete("CurrentUser");
                Response.Cookies.Delete("CurrentUserId");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account account)
        {           
            try
            {
                var logAccount = _authenticationService.Login(account.Username, account.Password, Role.Inspector);                
                Response.Cookies.Append("CurrentUserId", logAccount.Id.ToString());
                Response.Cookies.Append("CurrentUser", logAccount.Username);
                return RedirectToAction("Index", "Home");

            }
            catch(AuthenticationException)
            {                
                TempData["LoginError"] = "Onjuiste gebruikersnaam en/of wachtwoord";             
            }
            catch (NotAuthorizedException)
            {
                TempData["LoginError"] = "Dit account is niet geautoriseerd om hier in te loggen";
            }
            
            return View();
        }
    }
}
