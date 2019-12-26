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
        private IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account account)
        {
            account.Role = Role.Inspector;

            try
            {
                var logAccount = _authenticationService.Login(account.Username, account.Password, account.Role);
                return RedirectToAction("Index", "Home");

            }
            catch(Exception ex)
            {
                String errorMessage = ex.GetType().ToString();

                switch (ex.GetType().ToString())
                {
                    case "Festispec.Models.Exception.AuthenticationException":
                        errorMessage = "Onjuiste gebruikersnaam en/of wachtwoord";
                        break;
                    case "Festispec.Models.Exception.NotAuthorizedException":
                        errorMessage = "Dit account is niet geautoriseerd om hier in te loggen";
                        break;
                    default:
                        errorMessage = "Er ging iets fout: " + ex.GetType().ToString() + '"';
                        break;
                }

                TempData["LoginError"] = errorMessage;             
            }
            
            return View();
        }
    }
}
