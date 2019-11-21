using Festispec.DomainServices.Interfaces;
using Festispec.Models.EntityMapping;
using Festispec.Models.Questions;
using System.Threading.Tasks;
using System.Linq;
using Festispec.Models.Exception;
using Festispec.Models;
using System.Data.Entity;
using System.Collections.ObjectModel;

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
                throw new EntityNotFoundException();

            foreach (MultipleChoiceQuestion q in questionnaire.Questions.OfType<MultipleChoiceQuestion>())
                q.OptionCollection = new ObservableCollection<StringObject>(q.Options.Split(",").Select(str => new StringObject(str)));

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

        public Question GetQuestionFromQuestionnaire(Questionnaire questionnaire, int questionId)
        {
            var questionnaireFromDb = _db.Questionnaires.Include(x => x.Questions).FirstOrDefault(q => q.Id == questionnaire.Id);
            var question = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            return question;
        }
        
        public async Task<(bool, Question)> AddQuestion(Questionnaire questionnaire, Question question)
        {
            var questionnaireFromDb = _db.Questionnaires.FirstOrDefault(q => q.Id == questionnaire.Id);

            if (!question.Validate())
                throw new InvalidDataException();

            questionnaireFromDb.Questions.Add(question);

            return (await _db.SaveChangesAsync() == 1, question);
        }

        public async Task<bool> RemoveQuestion(int questionId)
        {
            var question = _db.Questions.Include(x => x.Answers).FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            if (question.Answers.Count() > 0)
                throw new QuestionHasAnswersException();

            if (_db.Questions.OfType<ReferenceQuestion>().Include(x => x.Question).Count(x => x.Question.Id == questionId) > 0)
                throw new QuestionHasReferencesException();

            _db.Questions.Remove(question);

            return await _db.SaveChangesAsync() == 1;
        }

        public async Task<Questionnaire> CopyQuestionnaire(int questionnaireId)
        {
            Questionnaire oldQuestionnaire = GetQuestionnaire(questionnaireId);

            var newQuestionnaire = await CreateQuestionnaire($"{oldQuestionnaire.Name} Copy", oldQuestionnaire.Festival);

            oldQuestionnaire.Questions.ToList().ForEach(async e =>  await AddQuestion(newQuestionnaire, new ReferenceQuestion(e.Contents, newQuestionnaire, e)));

            await _db.SaveChangesAsync();

            return newQuestionnaire;
        }
    }
}
