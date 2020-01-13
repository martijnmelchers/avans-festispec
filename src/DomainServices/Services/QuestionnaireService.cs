using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Helpers;
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
            var questionnaire = _db.Questionnaires
                .Include(x => x.Questions)
                .FirstOrDefault(q => q.Id == questionnaireId);

            if (questionnaire == null)
                throw new EntityNotFoundException();

            foreach (var q in questionnaire.Questions.OfType<MultipleChoiceQuestion>())
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
            if (await _db.Questionnaires.Include(x => x.Festival)
                .AnyAsync(x => x.Name == name && x.Festival.Id == festivalId))
                throw new EntityExistsException();

            var festival = await _db.Festivals.FirstOrDefaultAsync(f => f.Id == festivalId);

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

            if (questionnaire.Questions.Any(q => q.Answers.Count > 0))
                throw new QuestionHasAnswersException();

            _db.Questionnaires.Remove(questionnaire);

            await _db.SaveChangesAsync();
        }

        public async Task<Questionnaire> CopyQuestionnaire(int questionnaireId, string questionnaireName)
        {
            var oldQuestionnaire = GetQuestionnaire(questionnaireId);

            var newQuestionnaire =
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
            var questionnaire = _db.Questionnaires
                .Include(x => x.Questions)
                .FirstOrDefault(q => q.Id == questionnaireId);

            if (questionnaire == null)
                throw new EntityNotFoundException();

            var question = questionnaire.Questions.FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            return question;
        }

        public List<Question> GetQuestionsFromQuestionnaire(int questionnaireId)
        {
            var questions = _db.Questions
                .Include(x => x.Answers)
                .Where(q => q.Questionnaire.Id == questionnaireId)
                .ToList();

            foreach (var q in questions.OfType<MultipleChoiceQuestion>())
                q.StringToObjects();

            return questions;
        }

        public async Task<Question> AddQuestion(int questionnaireId, Question question)
        {
            var questionnaire = _db.Questionnaires.FirstOrDefault(q => q.Id == questionnaireId);

            question.Questionnaire = questionnaire;

            if (questionnaire == null)
                throw new EntityNotFoundException();

            if (!question.Validate())
                throw new InvalidDataException();

            questionnaire.Questions.Add(question);
            await _db.SaveChangesAsync();

            return question;
        }

        public async Task<bool> RemoveQuestion(int questionId)
        {
            var question = _db.Questions.Include(x => x.Answers).FirstOrDefault(q => q.Id == questionId);

            if (question == null)
                throw new EntityNotFoundException();

            if (question.Answers.Any())
                throw new QuestionHasAnswersException();

            if (_db.Questions.OfType<ReferenceQuestion>().Include(x => x.Question)
                    .Count(x => x.Question.Id == questionId) > 0)
                throw new QuestionHasReferencesException();

            _db.Questions.Remove(question);

            return await _db.SaveChangesAsync() > 1;
        }

        public async Task<Answer> CreateAnswer(Answer answer)
        {
            if (!answer.Validate())
                throw new InvalidDataException();

            _db.Answers.Add(answer);

            await _db.SaveChangesAsync();

            return answer;
        }

        public async Task<TAnswer> GetAnswer<TAnswer>(int id) where TAnswer : Answer
        {
            return await _db.Answers.OfType<TAnswer>().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return await _db.Questions.FirstOrDefaultAsync(e => e.Id == questionId);
        }

        #endregion Question Management


        #region inspection

        public async Task<List<PlannedInspection>> GetPlannedInspections(int employeeId)
        {
            var plannedInspections = await _db.PlannedInspections
                .Include(e => e.Employee)
                .Where(e => e.Employee.Id == employeeId &&
                            QueryHelpers.TruncateTime(e.StartTime) == QueryHelpers.TruncateTime(DateTime.Now))
                .ToListAsync();

            if (plannedInspections.Count < 1)
                throw new EntityNotFoundException();

            return plannedInspections;
        }


        public async Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {
            var plannedInspection = await _db.PlannedInspections
                .Include(pi => pi.Festival)
                .Include(pi => pi.Festival.Address)
                .FirstOrDefaultAsync(e => e.Id == plannedInspectionId);

            if (plannedInspection == null)
                throw new EntityNotFoundException();

            return plannedInspection;
        }

        #endregion

        [ExcludeFromCodeCoverage]
        public void Sync()
        {
            var db = _syncService.GetSyncContext();

            var questionnaires = db.Questionnaires
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