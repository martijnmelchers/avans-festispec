using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
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
            ViewBag["GetAvailabilityDays"] = JsonConvert.SerializeObject(await ConvertAvailibiltyToJson());
            return View();
        }

        public async Task<Dictionary<int, int>> ConvertAvailibiltyToJson()
        {
            var dictionary = new Dictionary<int, int>();
            var availibilityDictionary = await _availibilityService.GetUnavailabilitiesForFuture(_currentEmployeeId, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

            foreach (var availability in availibilityDictionary)
            {
                if (!availability.Value.IsAvailable)
                    dictionary.Add(availability.Key, 1);
                
            }

            return dictionary;
        }


        //[HttpPost]
        //public async task<iactionresult> index(availability avalability)
        //{
        //    try
        //    {
        //        await
        //    }
        //    catch (datehaspassedexception)
        //    {
        //        tempdata["dateerror"] = "datum mag niet in het verleden zijn!";
        //        return view("index");
        //    }

        //    return view("better");
        //}
    }
}