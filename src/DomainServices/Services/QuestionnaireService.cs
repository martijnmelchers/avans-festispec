using Festispec.DomainServices.Interfaces;
using Festispec.Models.EntityMapping;
using Festispec.Models.Questions;
using System.Threading.Tasks;
using System.Linq;
using Festispec.Models.Exception;
using Festispec.Models;
using System.Data.Entity;

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
            var questionnaire = _db.Questionnaires.Include(x => x.Questions).FirstOrDefault(q => q.Id == questionnaireId);

            if (questionnaire == null)
                throw new QuestionnaireNotFoundException();

            return questionnaire;
        }
        public async Task<Questionnaire> CreateQuestionnaire(string name, Festival festival)
        {
            var existing = _db.Questionnaires.Include(x => x.Festival).FirstOrDefault(x => x.Name == name && x.Festival.Id == festival.Id);

            if (existing != null)
                throw new EntityExistsException();

            var questionnaire = new Questionnaire(name, festival);

            if (!questionnaire.Validate())
                throw new InvalidDataException();

            _db.Questionnaires.Add(questionnaire);

            await _db.SaveChangesAsync();

            return questionnaire;
        }

        public async Task RemoveQuestionnaire(int questionnaireId)
        {
            var questionnaire = GetQuestionnaire(questionnaireId);

            if (questionnaire.Questions.FirstOrDefault(q => q.Answers.Count > 0) != null)
                throw new QuestionHasAnswersException();

            _db.Questionnaires.Remove(questionnaire);

            await _db.SaveChangesAsync();
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

            if (_db.Answers.Include(x => x.Question).Count(a => a.Question.Id == questionId) > 0)
                throw new QuestionHasAnswersException();

            if (_db.Questions.OfType<ReferenceQuestion>().Include(x => x.Question).Count(x => x.Question.Id == questionId) > 0)
                throw new QuestionHasReferencesException();

            questionnaire.Questions.Remove(question);

            await _db.SaveChangesAsync();
        }

        public async Task CopyQuestionnaire(int questionnaireId)
        {
            Questionnaire oldQuestionnaire = GetQuestionnaire(questionnaireId);

            var newQuestionnaire = await CreateQuestionnaire($"{oldQuestionnaire.Name} Copy", oldQuestionnaire.Festival);

            oldQuestionnaire.Questions.ToList().ForEach(async e =>  await AddQuestion(newQuestionnaire, new ReferenceQuestion(e.Contents, newQuestionnaire, e)));

            await _db.SaveChangesAsync();
        }
    }
}
