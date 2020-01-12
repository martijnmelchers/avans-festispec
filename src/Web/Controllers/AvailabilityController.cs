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

        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];
            ViewBag.SuccesBody = JsonConvert.SerializeObject(await ConvertAvailabilityToJson());
            return View();
        }

        public async Task<Dictionary<string, int>> ConvertAvailabilityToJson()
        {
            var availabilityDictionary = await _availabilityService.GetUnavailabilitiesForFuture(int.Parse(Request.Cookies["CurrentUserID"]), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

            return availabilityDictionary
                .Where(availability => !availability.Value.IsAvailable || !availabilityDictionary.ContainsKey(availability.Key))
                .ToDictionary(availability => $"{availability.Key}000", availability => 1);
        }


        [HttpPost]
        public async Task<IActionResult> Index(String j)
        {
            var dateTimes = new List<DateTime>();
            foreach (var item in Request.Form.Keys)
            {
                var dates = Request.Form[item].ToString();
                dateTimes.AddRange(dates.Split(',').Select(s => DateTime.Parse(s, new System.Globalization.CultureInfo("nl-NL"))));
            }

            foreach (var time in dateTimes)
            {
                var existing = _availabilityService.GetUnavailabilityForDay(int.Parse(Request.Cookies["CurrentUserID"]), time);
                if (existing == null)
                    try
                    {
                        await _availabilityService.AddUnavailabilityEntireDay(int.Parse(Request.Cookies["CurrentUserID"]), time, "test");
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                else
                {
                    try
                    {
                        await _availabilityService.RemoveUnavailability(existing.Id);
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }


            ViewBag.SuccesBody = JsonConvert.SerializeObject(await ConvertAvailabilityToJson());
            return View();
        }
    }
}