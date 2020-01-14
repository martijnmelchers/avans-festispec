using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Festispec.Web.Controllers
{
    public class InspectionController : Controller
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly AnswerFactory _answerFactory;

        public InspectionController(IQuestionnaireService questionnaireService, AnswerFactory answerFactory)
        {
            _questionnaireService = questionnaireService;
            _answerFactory = answerFactory;
        }

        // GET: Inspection
        public async Task<ActionResult> Index()
        {
            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];

            return View(await _questionnaireService.GetPlannedInspections(int.Parse(Request.Cookies["CurrentUserId"])));
        }

        // GET: Inspection/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var plannedInspection = await _questionnaireService.GetPlannedInspection(id);
            var answers = plannedInspection.Answers
                .Where(e => e.PlannedInspection.Employee.Id == int.Parse(Request.Cookies["CurrentUserId"]))
                .ToList();

            foreach (var question in plannedInspection.Questionnaire.Questions)
            {
                //check if question exists
                var modifiedQuestion = question;
                while (modifiedQuestion is ReferenceQuestion referenceQuestion)
                {
                    modifiedQuestion = referenceQuestion.Question;
                    modifiedQuestion.Id = referenceQuestion.Id;
                    modifiedQuestion.Contents = referenceQuestion.Contents;
                    modifiedQuestion.Answers = referenceQuestion.Answers;

                    foreach (var item in modifiedQuestion.Answers)
                        item.Question = modifiedQuestion;
                }

                if (modifiedQuestion.Answers.Any(e => e.PlannedInspection.Id == plannedInspection.Id)) continue;

                var answer = _answerFactory.GetAnswer(modifiedQuestion);
                answer.Question = modifiedQuestion;
                answer.PlannedInspection = plannedInspection;

                answers.Add(answer);
            }

            ViewBag.answers = answers;
            return View(plannedInspection);
        }

        //String answer
        [HttpPost]
        public async Task<ActionResult> SaveStringAnswer(StringAnswer stringAnswer)
        {
            var questionId = int.Parse(Request.Form["QuestionId"].ToString());

            if (stringAnswer.Id != 0)
            {
                var answer = await _questionnaireService.GetAnswer<StringAnswer>(stringAnswer.Id);

                if (answer != null)
                    answer.AnswerContents = stringAnswer.AnswerContents;

                await _questionnaireService.SaveChangesAsync();
            }
            else
            {
                stringAnswer.Question = await _questionnaireService.GetQuestion(questionId);
                stringAnswer.PlannedInspection =
                    await _questionnaireService.GetPlannedInspection(stringAnswer.PlannedInspection.Id);

                await _questionnaireService.CreateAnswer(stringAnswer);
            }

            return RedirectToAction("Details", new { id = stringAnswer.PlannedInspection.Id });
        }

        //DrawQuestion answer
        [HttpPost]
        public async Task<ActionResult> SaveDrawAnswer(FileAnswer fileAnswer)
        {
            var questionId = int.Parse(Request.Form["QuestionId"].ToString());

            if (fileAnswer.Id != 0)
            {
                var answer = await _questionnaireService.GetAnswer<FileAnswer>(fileAnswer.Id);

                if (answer != null)
                    answer.UploadedFilePath = fileAnswer.UploadedFilePath;

                await _questionnaireService.SaveChangesAsync();
            }
            else
            {
                fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);
                fileAnswer.PlannedInspection =
                    await _questionnaireService.GetPlannedInspection(fileAnswer.PlannedInspection.Id);

                fileAnswer = await _questionnaireService.CreateAnswer(fileAnswer) as FileAnswer;
            }


            return RedirectToAction("Draw", "Draw",new { id = fileAnswer?.Id });
        }

        //file answer
        [HttpPost]
        public async Task<ActionResult> SaveFileAnswer(IFormFile file)
        {
            var questionId = int.Parse(Request.Form["QuestionId"].ToString());
            var answerId = int.Parse(Request.Form["Id"]);
            var plannedInspectionId = int.Parse(Request.Form["PlannedInspectionId"].ToString());

            var path = (await UploadFile(file)).Replace(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\"), "");
            var comment = Request.Form["AnswerContents"].ToString();

            if (answerId != 0)
            {
                var answer = await _questionnaireService.GetAnswer<FileAnswer>(answerId);

                if (answer != null)
                {
                    answer.UploadedFilePath = path;
                    answer.AnswerContents = comment;
                }

                await _questionnaireService.SaveChangesAsync();
            }
            else
            {
                var fileAnswer = new FileAnswer
                {
                    Question = await _questionnaireService.GetQuestion(questionId),
                    PlannedInspection = await _questionnaireService.GetPlannedInspection(plannedInspectionId)
                };

                await _questionnaireService.CreateAnswer(fileAnswer);
            }


            return RedirectToAction("Details", new { id = plannedInspectionId });
        }

        //MultipleChoice Answer
        [HttpPost]
        public async Task<ActionResult> SaveMultipleChoiceAnswer(MultipleChoiceAnswer multipleChoiceAnswer)
        {
            var questionId = int.Parse(Request.Form["QuestionId"].ToString());

            if (multipleChoiceAnswer.Id != 0)
            {
                var answer = await _questionnaireService.GetAnswer<MultipleChoiceAnswer>(multipleChoiceAnswer.Id);

                if (answer != null)
                    answer.MultipleChoiceAnswerKey = multipleChoiceAnswer.MultipleChoiceAnswerKey;

                await _questionnaireService.SaveChangesAsync();
            }
            else
            {
                multipleChoiceAnswer.Question = await _questionnaireService.GetQuestion(questionId);
                multipleChoiceAnswer.PlannedInspection =
                    await _questionnaireService.GetPlannedInspection(multipleChoiceAnswer.PlannedInspection.Id);

                await _questionnaireService.CreateAnswer(multipleChoiceAnswer);
            }

            return RedirectToAction("Details", new { id = multipleChoiceAnswer.PlannedInspection.Id });
        }

        //Numeric Answer
        [HttpPost]
        public async Task<ActionResult> SaveNumericAnswer(NumericAnswer numericAnswer)
        {
            var questionId = int.Parse(Request.Form["QuestionId"].ToString());
            if (numericAnswer.Id != 0)
            {
                var answer = await _questionnaireService.GetAnswer<NumericAnswer>(numericAnswer.Id);

                if (answer != null)
                    answer.IntAnswer = numericAnswer.IntAnswer;

                await _questionnaireService.SaveChangesAsync();
            }
            else
            {
                numericAnswer.Question = await _questionnaireService.GetQuestion(questionId);
                numericAnswer.PlannedInspection =
                    await _questionnaireService.GetPlannedInspection(numericAnswer.PlannedInspection.Id);

                await _questionnaireService.CreateAnswer(numericAnswer);
            }

            return RedirectToAction("Details", new { id = numericAnswer.PlannedInspection.Id });
        }

        private static async Task<string> UploadFile(IFormFile file)
        {
            if (file == null || file.Length <= 0) return null;

            var fileName = Path.GetFileName(file.FileName);

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            
            var filePath = Path.Combine(uploadPath, fileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filePath;
        }
    }
}