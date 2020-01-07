using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Festispec.Models.Questions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly FestispecContext _db;

        public QuestionnaireService(FestispecContext db)
        {
            _db = db;
        }

        #region Questionnaire Management

        public Questionnaire GetQuestionnaire(int questionnaireId)
        {
            var questionnaire = _db.Questionnaires.Include(x => x.Questions).FirstOrDefault(q => q.Id == questionnaireId);

            if (questionnaire == null)
                throw new EntityNotFoundException();

            foreach (MultipleChoiceQuestion q in questionnaire.Questions.OfType<MultipleChoiceQuestion>())
                q.StringToObjects();

            return questionnaire;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public async Task<Questionnaire> CreateQuestionnaire(string name, Festival festival)
        {
            var existing = await _db.Questionnaires.Include(x => x.Festival).FirstOrDefaultAsync(x => x.Name == name && x.Festival.Id == festival.Id);

            if (existing != null)
                throw new EntityExistsException();

            var questionnaire = new Questionnaire(name, festival);

            if (!questionnaire.Validate())
                throw new InvalidDataException();

            _db.Questionnaires.Add(questionnaire);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return questionnaire;
        }

        public async Task RemoveQuestionnaire(int questionnaireId)
        {
            var questionnaire = GetQuestionnaire(questionnaireId);

            if (questionnaire.Questions.FirstOrDefault(q => q.Answers.Count > 0) != null)
                throw new QuestionHasAnswersException();

            _db.Questionnaires.Remove(questionnaire);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();
        }

        public async Task<Questionnaire> CopyQuestionnaire(int questionnaireId)
        {
            Questionnaire oldQuestionnaire = GetQuestionnaire(questionnaireId);

            var newQuestionnaire = await CreateQuestionnaire($"{oldQuestionnaire.Name} Copy", oldQuestionnaire.Festival);

            oldQuestionnaire.Questions.ToList().ForEach(async e => await AddQuestion(newQuestionnaire.Id, new ReferenceQuestion(e.Contents, newQuestionnaire, e)));

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return newQuestionnaire;
        }

        #endregion Questionnaire Management

        #region Question Management

        public Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId)
        {
            var questionnaire = _db.Questionnaires.Include(x => x.Questions).FirstOrDefault(q => q.Id == questionnaireId);
            var question = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            return question;
        }

        public List<Question> GetQuestionsFromQuestionnaire(int questionnaireId)
        {
            var questions = _db.Questions.Include(x => x.Answers).Where(q => q.Questionnaire.Id == questionnaireId).ToList();

            if (questions == null)
                throw new EntityNotFoundException();

            foreach (MultipleChoiceQuestion q in questions.OfType<MultipleChoiceQuestion>())
                q.StringToObjects();

            return questions;
        }

        public async Task<Question> AddQuestion(int questionnaireId, Question question)
        {
            var questionnaire = _db.Questionnaires.FirstOrDefault(q => q.Id == questionnaireId);

            if (!question.Validate())
                throw new InvalidDataException();

            questionnaire.Questions.Add(question);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return question;
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

            return await _db.SaveChangesAsync() > 1;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task<Answer> CreateAnswer(Answer answer)
        {
            if (!answer.Validate())
                throw new InvalidDataException();

            _db.Answers.Add(answer);

            await _db.SaveChangesAsync();

            return answer;
        }

        public List<Answer> getAnswers()
        {
            return _db.Answers.ToList();
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return await _db.Questions.FirstOrDefaultAsync(e => e.Id == questionId);
        }

        #endregion Question Management
    }
}