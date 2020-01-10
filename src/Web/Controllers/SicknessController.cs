using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SicknessController : Controller
    {
        private ISicknessService _sicknessService;

        public SicknessController(ISicknessService sicknessService)
        {
            _sicknessService = sicknessService;
        }
        public IActionResult Index()
        {
            if (Request.Cookies["CurrentUserId"] == null)
                return RedirectToAction("Login", "Authentication");

            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];
            if (_sicknessService.IsSick(int.Parse(Request.Cookies["CurrentUserID"])))
                return View("Better");
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Availability avalability)
        {
            try
            {
                await _sicknessService.AddAbsense(int.Parse(Request.Cookies["CurrentUserID"]), avalability.Reason, avalability.EndTime);
            }
            catch (DateHasPassedException)
            {
                TempData["DateError"] = "Datum mag niet in het verleden zijn!";
                return View("Index");
            }
            catch(Exception e)
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