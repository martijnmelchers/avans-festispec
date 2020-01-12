using System;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Microsoft.AspNetCore.Mvc;

namespace Festispec.Web.Controllers
{
    public class SicknessController : Controller
    {
        private readonly ISicknessService _sicknessService;

        public SicknessController(ISicknessService sicknessService)
        {
            _sicknessService = sicknessService;
        }
        public IActionResult Index()
        {
            if (Request.Cookies["CurrentUserId"] == null)
                return RedirectToAction("Login", "Authentication");

            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];
            return _sicknessService.IsSick(int.Parse(Request.Cookies["CurrentUserID"])) ? View("Better") : View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Availability availability)
        {
            try
            {
                await _sicknessService.AddAbsense(int.Parse(Request.Cookies["CurrentUserID"]), availability.Reason, availability.EndTime);
            }
            catch (DateHasPassedException)
            {
                TempData["DateError"] = "Datum mag niet in het verleden zijn!";
                return View("Index");
            }
            catch(Exception)
            {
                TempData["DateError"] = "Er ging iets fout";
                return View("Index");
            }
           
            return View("Better");
        }

        public async Task<IActionResult> Better()
        {
            if (Request.Cookies["CurrentUserId"] == null)
                return RedirectToAction("Login", "Authentication");
            
            await _sicknessService.EndAbsense(int.Parse(Request.Cookies["CurrentUserID"]));
            return View("Index");
        }
    }
}