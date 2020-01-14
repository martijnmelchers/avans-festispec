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
            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];
            return _sicknessService.IsSick(int.Parse(Request.Cookies["CurrentUserID"])) ? View("Better") : View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Availability availability)
        {
            try
            {
                await _sicknessService.AddAbsence(int.Parse(Request.Cookies["CurrentUserID"]), availability.Reason, availability.EndTime);
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
            await _sicknessService.EndAbsence(int.Parse(Request.Cookies["CurrentUserID"]));
            return View("Index");
        }
    }
}