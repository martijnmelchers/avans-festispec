using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
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
            var plannedInspection = _questionnaireService.GetPlannedInspections(id);
            var answers = new List<Answer>(plannedInspection.Answers);
            foreach (var question in plannedInspection.Questionnaire.Questions)
            {
                //check if question exists
                var question1 = question;
                if (question is Festispec.Models.Questions.ReferenceQuestion)
                {
                    question1 = (question as Festispec.Models.Questions.ReferenceQuestion).Question;
                    question1.Id = question.Id;
                    question1.Contents = question.Contents;
                }

                if (!question1.Answers.Any(e=> e.PlannedInspection.Id == plannedInspection.Id))
                {
                   
                    
                    switch (question1)
                    {
                        case Festispec.Models.Questions.NumericQuestion nq:
                            answers.Add(new Festispec.Models.Answers.NumericAnswer() { Question = question, PlannedInspection = plannedInspection });
                            break;
                        case Festispec.Models.Questions.RatingQuestion rq:
                            answers.Add(new Festispec.Models.Answers.NumericAnswer() { Question = question, PlannedInspection = plannedInspection });
                            break;
                        case Festispec.Models.Questions.MultipleChoiceQuestion mq:
                            answers.Add(new Festispec.Models.Answers.MultipleChoiceAnswer() { Question = question, PlannedInspection = plannedInspection });
                            break;
                        case Festispec.Models.Questions.StringQuestion sq:
                            answers.Add(new Festispec.Models.Answers.StringAnswer() { Question = question, PlannedInspection = plannedInspection });
                            break;
                        case Festispec.Models.Questions.DrawQuestion dq:
                            answers.Add(new Festispec.Models.Answers.FileAnswer() { Question = question, PlannedInspection = plannedInspection });
                            break;
                        case Festispec.Models.Questions.UploadPictureQuestion upq:
                            answers.Add(new Festispec.Models.Answers.FileAnswer() { Question = question, PlannedInspection = plannedInspection });
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
            stringAnswer.Question = _questionnaireService.GetPlannedInspections().FirstOrDefault(e => e.Id == stringAnswer.PlannedInspection.Id).Questionnaire.Questions.FirstOrDefault(e => e.Id == questionId);
            stringAnswer.PlannedInspection = _questionnaireService.GetPlannedInspections(stringAnswer.PlannedInspection.Id);

            if (stringAnswer.Id != 0)
                (_questionnaireService.getAnswers().FirstOrDefault(e => e.Id == stringAnswer.Id) as StringAnswer).AnswerContents = stringAnswer.AnswerContents;
            else await _questionnaireService.CreateAnswer(stringAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = stringAnswer.PlannedInspection.Id });
        }
        //file answer
        [HttpPost]
        public async Task<ActionResult> SaveFileAnswer(IFormFile file)
        {
            FileAnswer fileAnswer = new FileAnswer();
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            fileAnswer.PlannedInspection = _questionnaireService.GetPlannedInspections(int.Parse(Request.Form["PlannedInspectionId"].ToString()));
            fileAnswer.Question = _questionnaireService.GetPlannedInspections().FirstOrDefault(e => e.Id == fileAnswer.PlannedInspection.Id).Questionnaire.Questions.FirstOrDefault(e => e.Id == questionId);

            var filePath = await UploadFile(file);
            fileAnswer.UploadedFilePath = filePath;

            if (fileAnswer.Id != 0)
                (_questionnaireService.getAnswers().FirstOrDefault(e => e.Id == fileAnswer.Id) as FileAnswer).UploadedFilePath = fileAnswer.UploadedFilePath;
            else await _questionnaireService.CreateAnswer(fileAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = fileAnswer.PlannedInspection.Id });
        }

        //MultipleChoice Answer
        [HttpPost]
        public async Task<ActionResult> SaveMultipleChoiceAnswer(MultipleChoiceAnswer multipleChoiceAnswer)
        {
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            multipleChoiceAnswer.Question = _questionnaireService.GetPlannedInspections().FirstOrDefault(e => e.Id == multipleChoiceAnswer.PlannedInspection.Id).Questionnaire.Questions.FirstOrDefault(e => e.Id == questionId);
            multipleChoiceAnswer.PlannedInspection = _questionnaireService.GetPlannedInspections(multipleChoiceAnswer.PlannedInspection.Id);
            if (multipleChoiceAnswer.Id != 0)
                (_questionnaireService.getAnswers().FirstOrDefault(e => e.Id == multipleChoiceAnswer.Id) as MultipleChoiceAnswer).MultipleChoiceAnswerKey = multipleChoiceAnswer.MultipleChoiceAnswerKey;
            else await _questionnaireService.CreateAnswer(multipleChoiceAnswer);
            await _questionnaireService.SaveChangesAsync();
            return RedirectToAction("Details", new { id = multipleChoiceAnswer.PlannedInspection.Id });
        }

        //Numeric Answer
        [HttpPost]
        public async Task<ActionResult> SaveNumericAnswer(NumericAnswer numericAnswer)
        {
            
            int questionId = int.Parse(Request.Form["QuestionId"].ToString());
            numericAnswer.Question = _questionnaireService.GetPlannedInspections().FirstOrDefault(e => e.Id == numericAnswer.PlannedInspection.Id).Questionnaire.Questions.FirstOrDefault(e => e.Id == questionId);
            numericAnswer.PlannedInspection = _questionnaireService.GetPlannedInspections(numericAnswer.PlannedInspection.Id);
            if (numericAnswer.Id != 0)
            {
                (_questionnaireService.getAnswers().FirstOrDefault(e => e.Id == numericAnswer.Id) as NumericAnswer).IntAnswer = numericAnswer.IntAnswer;
                await _questionnaireService.SaveChangesAsync();
            }
            else await _questionnaireService.CreateAnswer(numericAnswer);
            return RedirectToAction("Details", new { id = numericAnswer.PlannedInspection.Id });
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
