using Festispec.DomainServices.Interfaces;
using Festispec.Models.EntityMapping;
using Festispec.Models.Questions;
using System.Threading.Tasks;
using System.Linq;
using Festispec.Models.Exception;
using Festispec.Models;

namespace Festispec.DomainServices.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly FestispecContext _db;

        public QuestionnaireService(FestispecContext db)
        {
            _db = db;
        }
        public Questionnaire GetQuestionnaire(int questionnaireId)
        {
            var questionnaire = _db.Questionnaires.FirstOrDefault(q => q.Id == questionnaireId);

            if (questionnaire == null)
                throw new QuestionnaireNotFoundException();

            return questionnaire;
        }
        public async Task<Questionnaire> CreateQuestionnaire(PlannedInspection plannedInspection)
        {
            var questionnaire = new Questionnaire()
            {
                PlannedInspection = plannedInspection
            };

            _db.Questionnaires.Add(questionnaire);
            await _db.SaveChangesAsync();

            return questionnaire;
        }
        
        public async Task AddQuestion(Questionnaire questionnaire, Question question)
        {
            questionnaire.Questions.Add(question);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveQuestion(Questionnaire questionnaire, int questionId)
        {
            var question = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            if(question == null)
                throw new QuestionNotFoundException();

            if (_db.Answers.Count(a => a.Question.Id == questionId) > 0)
                throw new QuestionHasAnswersException();

            questionnaire.Questions.Remove(question);

            await _db.SaveChangesAsync();
        }
        public async Task EditQuestion(Questionnaire questionnaire, Question question)
        {
            if (_db.Answers.Count(a => a.Question.Id == question.Id) > 0)
                throw new QuestionHasAnswersException();

            /*var question = questionnaire.Questions.FirstOrDefault(q => q.Id == question.Id);*/

        }
    }
}
