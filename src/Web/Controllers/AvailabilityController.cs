using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Festispec.Web.Controllers
{
    public class AvailabilityController : Controller
    {

        private IAvailabilityService _availibilityService;
        private int _currentEmployeeId;

        public AvailabilityController(IAvailabilityService availibilityService)
        {
            _availibilityService = availibilityService;
            //_currentEmployeeId = Int32.Parse(Request.Cookies["CurrentUserID"]);
            _currentEmployeeId = 1;
        }
        public async Task <IActionResult> Index()
        {
            ViewBag.SuccesBody = JsonConvert.SerializeObject(await ConvertAvailibiltyToJson());
            return View();
        }

        public async Task<Dictionary<long, int>> ConvertAvailibiltyToJson()
        {
            var dictionary = new Dictionary<long, int>();
            var availibilityDictionary = await _availibilityService.GetUnavailabilitiesForFuture(_currentEmployeeId, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

            foreach (var availability in availibilityDictionary)
            {
                if (!availability.Value.IsAvailable)
                    dictionary.Add(availability.Key, 1);
                
            }
            dictionary.Add(1578355200000, 1);
            return dictionary;
        }


        [HttpPost]
        public async Task<IActionResult> Index(List<DateTime> dateTimes)
        {
            try
            {
                foreach (DateTime time in dateTimes)
                {
                    await _availibilityService.AddUnavailabilityEntireDay(Int32.Parse(Request.Cookies["CurrentUserID"]), time, "");
                }
            }
            catch (DateHasPassedException)
            {
                TempData["DateError"] = "Datum mag niet in het verleden zijn!";
                return View("Index");
            }
            

            return View("Index");
        }
    }
}