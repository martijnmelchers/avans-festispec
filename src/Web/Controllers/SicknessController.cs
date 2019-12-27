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
        public async Task<IActionResult> Index(Availability avalability)
        {
            await _sicknessService.AddAbsense(avalability.Employee.Id, avalability.Reason, avalability.EndTime);
            return View("Better");
        }

        public async Task<IActionResult> Better()
        {
            await _sicknessService.EndAbsense(1);
            return View("Index");
        }
    }
}