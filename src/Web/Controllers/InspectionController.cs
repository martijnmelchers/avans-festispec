using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Festispec.Web.Controllers
{
    public class InspectionController : Controller
    {
        private IQuestionnaireService _questionnaireService { get; set; }
        public InspectionController(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
        }
        // GET: Inspection
        public ActionResult Index()
        {
            return View(_questionnaireService.GetPlannedInspections());
        }

        // GET: Inspection/Details/5
        public ActionResult Details(int id)
        {
            return View(_questionnaireService.GetPlannedInspections(id));
        }

        [HttpPost]
        public async Task<IActionResult> Details(int id, List<Answer> answers )
        {
            foreach (var item in answers)
            {
                if (ModelState.IsValid)
                {
                    await _questionnaireService.CreateAnswer(item);
                }
            }
            return View(_questionnaireService.GetPlannedInspections(id));
        }

        // GET: Inspection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inspection/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inspection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Inspection/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inspection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inspection/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}