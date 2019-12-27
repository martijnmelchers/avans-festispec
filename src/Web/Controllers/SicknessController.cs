using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
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
            if (_sicknessService.IsSick(1))
            {
                return View("Better");
            }
            
            return View();
        }

        [HttpPost]
        public IActionResult Index(Availability avalabillity)
        {
            _sicknessService.AddAbsense(avalabillity.Employee, avalabillity.Reason, avalabillity.EndTime);
            return View();
        }

        public IActionResult Better()
        {
            _sicknessService.EndAbsense(1);
            return View("Index");
        }
    }
}