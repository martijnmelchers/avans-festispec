using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Festispec.DomainServices.Helpers;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.EntityMapping;
using Festispec.Models.Questions;
namespace Festispec.DomainServices.Services
{
    class QuestionService: IQuestionService
    {
        private readonly FestispecContext _db;

        public QuestionService(FestispecContext db)
        {
            _db = db;
        }

        public List<Answer> GetAnswers(Question question)
        {
            return ModelMocks.Answers.Where(x => x.Question == question).ToList();
           
            List<Answer> questionAnswers = new List<Answer>();
            var answers = _db.Answers;
            return answers.Where(x => x.Question == question).ToList();
        }

        public List<Question> GetQuestions(Questionnaire questionnaire)
        {
            return ModelMocks.Questions.Where(x => x.Questionnaire == questionnaire).ToList();
            return _db.Questions.Where(x => x.Questionnaire == questionnaire).ToList();
        }

        public Questionnaire GetQuestionaire(int id)
        {
            return ModelMocks.Questionnaires.Where(x => x.Id == id).FirstOrDefault();
            return _db.Questionnaires.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
