using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Festispec.Web.Controllers
{
    public class DrawController : Controller
    {
        // GET: Draw
        public ActionResult Index()
        {
            return View();
        }

        // GET: Draw/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Draw/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Draw/Create
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

        // GET: Draw/Edit/5
        public ActionResult Draw(int id)
        {
            return View();
        }

        // POST: Draw/Edit/5
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

        // GET: Draw/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Draw/Delete/5
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