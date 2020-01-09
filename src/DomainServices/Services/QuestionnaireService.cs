using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.EntityMapping;
using Festispec.Models.Exception;
using Festispec.Models.Questions;

namespace Festispec.DomainServices.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly FestispecContext _db;
        private readonly ISyncService<Questionnaire> _syncService;

        public QuestionnaireService(FestispecContext db, ISyncService<Questionnaire> syncService)
        {
            _db = db;
            _syncService = syncService;
        }

        #region Questionnaire Management

        public Questionnaire GetQuestionnaire(int questionnaireId)
        {
            Questionnaire questionnaire = _db.Questionnaires.Include(x => x.Questions)
                .FirstOrDefault(q => q.Id == questionnaireId);

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

        public async Task<Questionnaire> CreateQuestionnaire(string name, int festivalId)
        {
            Questionnaire existing = await _db.Questionnaires.Include(x => x.Festival)
                .FirstOrDefaultAsync(x => x.Name == name && x.Festival.Id == festivalId);

            if (existing != null)
                throw new EntityExistsException();

            Festival festival = await _db.Festivals.FirstOrDefaultAsync(f => f.Id == festivalId);

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
            Questionnaire questionnaire = GetQuestionnaire(questionnaireId);

            if (questionnaire.Questions.FirstOrDefault(q => q.Answers.Count > 0) != null)
                throw new QuestionHasAnswersException();

            _db.Questionnaires.Remove(questionnaire);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();
        }

        public async Task<Questionnaire> CopyQuestionnaire(int questionnaireId, string questionnaireName)
        {
            Questionnaire oldQuestionnaire = GetQuestionnaire(questionnaireId);

            Questionnaire newQuestionnaire =
                await CreateQuestionnaire(questionnaireName, oldQuestionnaire.Festival.Id);
            
            foreach (var e in oldQuestionnaire.Questions)
            {
                await AddQuestion(newQuestionnaire.Id, new ReferenceQuestion(e.Contents, newQuestionnaire, e));
            }

            await _db.SaveChangesAsync();

            return newQuestionnaire;
        }

        #endregion Questionnaire Management

        #region Question Management

        public Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId)
        {
            Questionnaire questionnaire = _db.Questionnaires.Include(x => x.Questions)
                .FirstOrDefault(q => q.Id == questionnaireId);
            Question question = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            return question;
        }

        public List<Question> GetQuestionsFromQuestionnaire(int questionnaireId)
        {
            List<Question> questions = _db.Questions.Include(x => x.Answers)
                .Where(q => q.Questionnaire.Id == questionnaireId).ToList();

            if (questions == null)
                throw new EntityNotFoundException();

            foreach (MultipleChoiceQuestion q in questions.OfType<MultipleChoiceQuestion>())
                q.StringToObjects();

            return questions;
        }

        public async Task<Question> AddQuestion(int questionnaireId, Question question)
        {
            Questionnaire questionnaire = _db.Questionnaires.FirstOrDefault(q => q.Id == questionnaireId);

            question.Questionnaire = questionnaire;

            if (!question.Validate())
                throw new InvalidDataException();

            questionnaire.Questions.Add(question);

            if (await _db.SaveChangesAsync() == 0)
                throw new NoRowsChangedException();

            return question;
        }

        public async Task<bool> RemoveQuestion(int questionId)
        {
            Question question = _db.Questions.Include(x => x.Answers).FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            if (question.Answers.Count() > 0)
                throw new QuestionHasAnswersException();

            if (_db.Questions.OfType<ReferenceQuestion>().Include(x => x.Question)
                    .Count(x => x.Question.Id == questionId) > 0)
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

        public List<Answer> GetAnswers()
        {
            return _db.Answers.Include(a => a.Question).ToList();
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return await _db.Questions.FirstOrDefaultAsync(e => e.Id == questionId);
        }

        #endregion Question Management

        public void Sync()
        {
            FestispecContext db = _syncService.GetSyncContext();
            
            List<Questionnaire> questionnaires = db.Questionnaires
                .Include(q => q.Festival)
                .Include(q => q.Questions)
                .Include(q => q.Questions.Select(qu => qu.Answers))
                .ToList();

            _syncService.Flush();
            _syncService.AddEntities(questionnaires);
            _syncService.SaveChanges();
        }
    }
}
