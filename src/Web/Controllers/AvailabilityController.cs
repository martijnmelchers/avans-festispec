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

        private IAvailabilityService _availabilityService;

        public AvailabilityController(IAvailabilityService availibilityService)
        {
            _availabilityService = availibilityService;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];
            try
            {
                ViewBag.SuccesBody = JsonConvert.SerializeObject(await ConvertAvailibiltyToJson());
            }
            catch (Exception e)
            {
                if (Request.Cookies["CurrentUserId"] == null)
                    return RedirectToAction("Login", "Authentication");
            }
            return View();
        }

        public async Task<Dictionary<string, int>> ConvertAvailibiltyToJson()
        {
            var dictionary = new Dictionary<string, int>();
            var availibilityDictionary = await _availabilityService.GetUnavailabilitiesForFuture(Int32.Parse(Request.Cookies["CurrentUserID"]), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));

            foreach (var availability in availibilityDictionary)
            {
                if (!availability.Value.IsAvailable || !availibilityDictionary.ContainsKey(availability.Key))
                    dictionary.Add($"{availability.Key}000", 1);

            }
            return dictionary;
        }


        [HttpPost]
        public async Task<IActionResult> Index(String j)
        {
            List<DateTime> dateTimes = new List<DateTime>();
            foreach (var item in Request.Form.Keys)
            {
                var test = Request.Form[item].ToString();
                foreach (var s in test.Split(','))
                {
                    dateTimes.Add(DateTime.Parse(s, new System.Globalization.CultureInfo("nl-NL")));
                }
            }

            foreach (DateTime time in dateTimes)
            {
                var existing = _availabilityService.GetUnavailabilityForDay(Int32.Parse(Request.Cookies["CurrentUserID"]), time);
                if (existing == null)
                    try
                    {
                        await _availabilityService.AddUnavailabilityEntireDay(Int32.Parse(Request.Cookies["CurrentUserID"]), time, "test");
                    }
                    catch (Exception)
                    {

                    }
                else
                {
                    try
                    {
                        await _availabilityService.RemoveUnavailablity(existing.Id);
                    }
                    catch (Exception)
                    {
                    }
                }
            }


            ViewBag.SuccesBody = JsonConvert.SerializeObject(await ConvertAvailibiltyToJson());
            return View();
        }
    }
}