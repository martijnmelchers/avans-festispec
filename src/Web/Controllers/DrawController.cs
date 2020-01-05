using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.Answers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Festispec.Web.Controllers
{
    public class DrawController : Controller
    {
        IQuestionnaireService _questionnaireService;
        public DrawController(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
        }
      

        // GET: Draw/Draw/5
        public ActionResult Draw(int id)
        {
            FileAnswer answer = _questionnaireService.getAnswers().FirstOrDefault(e=> e.Id == id) as FileAnswer;
            return View(answer);
        }

        // POST: Draw/Draw/5
        [HttpPost]
        public async Task<ActionResult> Draw(string imageData)
        {
            var request = Request.Form;
            FileAnswer fileAnswer = _questionnaireService.getAnswers().FirstOrDefault(e => e.Id == 1) as FileAnswer;

            var path = "test";

            string fileNameWitPath = path + DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png";
            using (FileStream fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(imageData);
                    bw.Write(data);
                    bw.Close();
                }
            }


            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { V = "test" });
        }



    }
}