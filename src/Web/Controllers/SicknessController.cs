using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SicknessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Availability avalabillity)
        {
            return View();
        }
    }
}