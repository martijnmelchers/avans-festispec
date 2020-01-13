using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Festispec.Web.Controllers
{
    public class DrawController : Controller
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IInspectionService _inspectionService;
        private readonly IConfiguration _configurationService;

        public DrawController(IQuestionnaireService questionnaireService, IInspectionService inspectionService,
            IConfiguration configurationService)
        {
            _questionnaireService = questionnaireService;
            _inspectionService = inspectionService;
            _configurationService = configurationService;
        }

        // GET: Draw/Draw/5
        public async  Task<ActionResult> Draw(int id)
        {
            ViewBag.URL = _configurationService["Urls:WebApp"];
            return View(await _questionnaireService.GetAnswer<FileAnswer>(id));
        }

        // POST: Draw/Draw/5
        [HttpPost]
        public async Task<ActionResult> Draw()
        {
            var questionId = int.Parse(Request.Form["QuestionId"]);
            var plannedInspectionId = int.Parse(Request.Form["plannedInspectionId"]);
            var plannedInspection = await _inspectionService.GetPlannedInspection(plannedInspectionId);
            var fileAnswer = plannedInspection.Answers
                .OfType<FileAnswer>()
                .FirstOrDefault(e => e.Question.Id == questionId);
            
            if (fileAnswer == null)
                return RedirectToAction("Details", "inspection", new { id = plannedInspection.Id });

            fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);


            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", $"{Guid.NewGuid()}.png");

            var formFile = Request.Form.Files[0];

            if (formFile.Length > 0)
            {
                await using var imageFile = new FileStream(filePath, FileMode.Create);
                await formFile.CopyToAsync(imageFile);
            }

            var answer = await _questionnaireService.GetAnswer<FileAnswer>(fileAnswer.Id);

            if (fileAnswer.Id != 0 && answer != null)
                answer.UploadedFilePath = filePath.Replace(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\"), "");
            else
                await _questionnaireService.CreateAnswer(fileAnswer);


            await _questionnaireService.SaveChangesAsync();

            return RedirectToAction("Details", "inspection", new { id = fileAnswer.PlannedInspection.Id });
        }
    }
}