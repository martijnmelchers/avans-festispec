using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.Web.Controllers
{
    public class InspectionController : Controller
    {
        private IQuestionnaireService _questionnaireService { get; set; }
        private IInspectionService _inspectionService { get; set; }

        public InspectionController(IQuestionnaireService questionnaireService, IInspectionService inspectionService)
        {
            _questionnaireService = questionnaireService;
            _inspectionService = inspectionService;
        }

        // GET: Inspection
        public async Task<ActionResult> Index()
        {
            ViewData["CurrentUser"] = Request.Cookies["CurrentUser"];
            List<PlannedInspection> plannedInspections;
            try
            {
                plannedInspections = await _questionnaireService.GetPlannedInspections(int.Parse(Request.Cookies["CurrentUserId"]));
            }
            catch (Exception e)
            {
                if (Request.Cookies["CurrentUserId"] != null)
                {
                    plannedInspections = new List<PlannedInspection>();
                }
                else
                {
                    return RedirectToAction("Login", "Authentication");
                }
            }
            return View(plannedInspections);
        }

        // GET: Inspection/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var plannedInspection = await _questionnaireService.GetPlannedInspection(id);
            List<Answer> answers;
            try
            {
                answers = new List<Answer>(plannedInspection.Answers.Where(e => e.PlannedInspection.Employee.Id == int.Parse(Request.Cookies["CurrentUserId"])));
            }
            catch (Exception)
            {
                if (Request.Cookies["CurrentUserId"] == null)
                     return RedirectToAction("Login", "Authentication");

                answers = new List<Answer>();
            }

            foreach (var question in plannedInspection.Questionnaire.Questions)
            {
                //check if question exists
                var question1 = question;
                if (question is Festispec.Models.Questions.ReferenceQuestion)
                {
                    question1 = (question as Festispec.Models.Questions.ReferenceQuestion).Question;
                    question1.Id = question.Id;
                    question1.Contents = question.Contents;
                    question1.Answers = question.Answers;

                    foreach (var item in question1.Answers)
                        item.Question = question1;

                }

                if (!question1.Answers.Any(e => e.PlannedInspection.Id == plannedInspection.Id))
                {
                    switch (question1)
                    {
                        case Festispec.Models.Questions.NumericQuestion _:
                            answers.Add(new NumericAnswer() { Question = question1, PlannedInspection = plannedInspection });
                            break;

                        case Festispec.Models.Questions.RatingQuestion _:
                            answers.Add(new NumericAnswer() { Question = question1, PlannedInspection = plannedInspection });
                            break;

                        case Festispec.Models.Questions.MultipleChoiceQuestion _:
                            answers.Add(new MultipleChoiceAnswer() { Question = question1, PlannedInspection = plannedInspection });
                            break;

                        case StringQuestion _:
                            answers.Add(new StringAnswer() { Question = question1, PlannedInspection = plannedInspection });
                            break;

                        case DrawQuestion _:
                            answers.Add(new FileAnswer() { Question = question1, PlannedInspection = plannedInspection });
                            break;

                        case UploadPictureQuestion _:
                            answers.Add(new FileAnswer() { Question = question1, PlannedInspection = plannedInspection });
                            break;

                        default:
                            break;
                    }
                }
            }
            ViewBag.answers = answers;
            return View(plannedInspection);
        }

        //String answer
        [HttpPost]
        public async Task<ActionResult> SaveStringAnswer(StringAnswer stringAnswer)
        {
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            stringAnswer.Question = await _questionnaireService.GetQuestion(questionId);
            stringAnswer.PlannedInspection = await _questionnaireService.GetPlannedInspection(stringAnswer.PlannedInspection.Id);

            if (stringAnswer.Id != 0)
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == stringAnswer.Id) as StringAnswer).AnswerContents = stringAnswer.AnswerContents;
            else await _questionnaireService.CreateAnswer(stringAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = stringAnswer.PlannedInspection.Id });
        }
        //DrawQuestion answer
        [HttpPost]
        public async Task<ActionResult> SaveDrawAnswer(FileAnswer fileAnswer)
        {
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);
            fileAnswer.PlannedInspection = await _questionnaireService.GetPlannedInspection(fileAnswer.PlannedInspection.Id);

            if (fileAnswer.Id != 0)
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = fileAnswer.UploadedFilePath;
            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Draw", "Draw", new { id = fileAnswer.PlannedInspection.Answers.FirstOrDefault(e=> e.Question.Id == questionId).Id });
        }

        //file answer
        [HttpPost]
        public async Task<ActionResult> SaveFileAnswer(IFormFile file)
        {
            FileAnswer fileAnswer = new FileAnswer();
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            fileAnswer.Id = int.Parse(Request.Form["Id".ToString()]);
            fileAnswer.Question = await _questionnaireService.GetQuestion(questionId);
            fileAnswer.PlannedInspection = await _questionnaireService.GetPlannedInspection(int.Parse(Request.Form["PlannedInspectionId"].ToString()));

            var filePath = await UploadFile(file);
            fileAnswer.UploadedFilePath = filePath;
            fileAnswer.AnswerContents = Request.Form["AnswerContents"].ToString();

            if (fileAnswer.Id != 0)
            {
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = fileAnswer.UploadedFilePath;
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).AnswerContents = fileAnswer.AnswerContents;

            }

            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = fileAnswer.PlannedInspection.Id });
        }

        //MultipleChoice Answer
        [HttpPost]
        public async Task<ActionResult> SaveMultipleChoiceAnswer(MultipleChoiceAnswer multipleChoiceAnswer)
        {
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            multipleChoiceAnswer.Question = await _questionnaireService.GetQuestion(questionId);
            multipleChoiceAnswer.PlannedInspection = await _questionnaireService.GetPlannedInspection(multipleChoiceAnswer.PlannedInspection.Id);
            if (multipleChoiceAnswer.Id != 0)
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == multipleChoiceAnswer.Id) as MultipleChoiceAnswer).MultipleChoiceAnswerKey = multipleChoiceAnswer.MultipleChoiceAnswerKey;
            else await _questionnaireService.CreateAnswer(multipleChoiceAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = multipleChoiceAnswer.PlannedInspection.Id });
        }

        //Numeric Answer
        [HttpPost]
        public async Task<ActionResult> SaveNumericAnswer(NumericAnswer numericAnswer)
        {
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            numericAnswer.Question = await _questionnaireService.GetQuestion(questionId);
            numericAnswer.PlannedInspection = await _questionnaireService.GetPlannedInspection(numericAnswer.PlannedInspection.Id);
            if (numericAnswer.Id != 0)
            {
                (_questionnaireService.GetAnswers().FirstOrDefault(e => e.Id == numericAnswer.Id) as NumericAnswer).IntAnswer = numericAnswer.IntAnswer;
                await _questionnaireService.SaveChangesAsync();
            }
            else await _questionnaireService.CreateAnswer(numericAnswer);
            return RedirectToAction("Details", new { id = numericAnswer.PlannedInspection.Id });
        }

        private async Task<string> UploadFile(IFormFile ufile)
        {
            if (ufile == null || ufile.Length <= 0) return null;
            var fileName = Path.GetFileName(ufile.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads", fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ufile.CopyToAsync(fileStream);
            }
            return filePath;
        }
    }
}