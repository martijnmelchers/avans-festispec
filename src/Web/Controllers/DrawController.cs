using Festispec.DomainServices.Interfaces;
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
            if (Request.Cookies["CurrentUserId"] == null)
                return RedirectToAction("Login", "Authentication");
            FileAnswer answer = _questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == id) as FileAnswer;
            return View(answer);
        }

        // POST: Draw/Draw/5
        [HttpPost]
        public async Task<ActionResult> Draw(string imageData)
        {
            JObject j = JObject.Parse(imageData);
            int questionId = int.Parse(j["QuestionId"].ToString());
            FileAnswer fileAnswer = _questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == questionId) as FileAnswer;

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
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = fileAnswer.UploadedFilePath;
            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();

            return RedirectToAction("Details", "inspection", new { id = fileAnswer.PlannedInspection.Id });
        }
    }
}