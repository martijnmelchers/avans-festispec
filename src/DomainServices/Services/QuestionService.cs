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
using System.Data.Entity;
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
            List<Answer> questionAnswers = new List<Answer>();
            var answers = _db.Answers;
            return answers.Where(x => x.Question == question).ToList();
        }

        public Questionnaire GetQuestionaire(int id)
        {
            //   return ModelMocks.Questionnaires.Where(x => x.Id == id).FirstOrDefault();
            return _db.Questionnaires.FirstOrDefault(x => x.Id == id);
        }

        public Festival GetFestival(int festivalId)
        {
            var festival = _db.Festivals.Include(f => f.Questionnaires).FirstOrDefault(f => f.Id == festivalId);
            if( festival != null)
            {
                return festival;
            }
            return null;
        }
    }
}
