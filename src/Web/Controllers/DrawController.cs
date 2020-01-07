using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Festispec.Web.Controllers
{
    public class DrawController : Controller
    {
        IQuestionnaireService _questionnaireService;
        IInspectionService _inspectionService;
        public DrawController(IQuestionnaireService questionnaireService, IInspectionService inspectionService)
        {
            _questionnaireService = questionnaireService;
            _inspectionService = inspectionService;
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
            JObject j = JObject.Parse(imageData);
            int questionId = int.Parse(j["QuestionId"].ToString());
            FileAnswer fileAnswer = _questionnaireService.getAnswers().FirstOrDefault(e => e.Id == questionId) as FileAnswer;

            fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);

            imageData = j["ImageData"].ToString();


            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png");

            var bytes = Convert.FromBase64String(imageData);
            using (var imageFile = new FileStream(filePath, FileMode.Create))
            {
                imageFile.Write(bytes, 0, bytes.Length);
                imageFile.Flush();
            }

            if (fileAnswer.Id != 0)
                (_questionnaireService.getAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = fileAnswer.UploadedFilePath;
            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();

            return RedirectToAction("Details", "inspection", new { id = fileAnswer.PlannedInspection.Id });





        }


        public async Task<ActionResult> SaveFileAnswer(IFormFile file)
        {
            FileAnswer fileAnswer = new FileAnswer();
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            fileAnswer.PlannedInspection = await _inspectionService.GetPlannedInspection(int.Parse(Request.Form["PlannedInspectionId"].ToString()));
            fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);

            var filePath = await UploadFile(file);
            fileAnswer.UploadedFilePath = filePath;

            if (fileAnswer.Id != 0)
                (_questionnaireService.getAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = fileAnswer.UploadedFilePath;
            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = fileAnswer.PlannedInspection.Id });
        }

        private async Task<string> UploadFile(IFormFile ufile)
        {
            if (ufile != null && ufile.Length > 0)
            {
                var fileName = Path.GetFileName(ufile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ufile.CopyToAsync(fileStream);
                }
                return filePath;
            }
            return null;
        }



    }
}