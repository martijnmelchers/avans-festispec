using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.Web.Controllers
{
    public class DrawController : Controller
    {
        private IQuestionnaireService _questionnaireService;
        private IInspectionService _inspectionService;

        public DrawController(IQuestionnaireService questionnaireService, IInspectionService inspectionService)
        {
            _questionnaireService = questionnaireService;
            _inspectionService = inspectionService;
        }

        // GET: Draw/Draw/5
        public ActionResult Draw(int id)
        {
            FileAnswer answer = _questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == id) as FileAnswer;
            return View(answer);
        }

        // POST: Draw/Draw/5
        [HttpPost]
        public async Task<ActionResult> Draw(string imageData)
        {
            int questionId = Convert.ToInt32(Request.Form["QuestionId"]);
            int plannedInspectionId = Convert.ToInt32(Request.Form["plannedInspectionId"]);
            PlannedInspection plannedInspection = await _inspectionService.GetPlannedInspection(plannedInspectionId);
            FileAnswer fileAnswer = plannedInspection.Answers.FirstOrDefault(e=> e.Question.Id == questionId) as FileAnswer;
            fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);


            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", DateTime.Now.ToString().Replace("/", "-").Replace(" ", "- ").Replace(":", "") + ".png");

            var formFile = Request.Form.Files[0];

            if (formFile.Length > 0)
            {
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(imageFile);
                }
            }

            if (fileAnswer.Id != 0)
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = filePath;
            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();

            return RedirectToAction("Details", "inspection", new { id = fileAnswer.PlannedInspection.Id });
        }
    }
}
