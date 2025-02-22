using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Exception;
using Festispec.Models.Questions;

namespace Festispec.DomainServices.Services.Offline
{
    [ExcludeFromCodeCoverage]
    public class OfflineQuestionnaireService : IQuestionnaireService
    {
        private readonly ISyncService<Questionnaire> _syncService;

        public OfflineQuestionnaireService(ISyncService<Questionnaire> syncService)
        {
            _syncService = syncService;
        }
        
        public Questionnaire GetQuestionnaire(int questionnaireId)
        {
            return _syncService.GetEntity(questionnaireId);
        }

        public Task<Questionnaire> CreateQuestionnaire(string name, int festivalId)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Question> AddQuestion(int questionnaireId, Question question)
        {
            throw new System.InvalidOperationException();
        }

        public Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId)
        {
            var questionFromQuestionnaire = _syncService.GetEntity(questionnaireId).Questions.FirstOrDefault(q => q.Id == questionId);
            
            if (questionFromQuestionnaire == null)
                throw new EntityNotFoundException();
            
            return questionFromQuestionnaire;
        }

        public Task<bool> RemoveQuestion(int questionId)
        {
            throw new System.InvalidOperationException();
        }

        public Task RemoveQuestionnaire(int questionnaireId)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Questionnaire> CopyQuestionnaire(int questionnaireId, string questionnaireName)
        {
            throw new System.InvalidOperationException();
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return (await _syncService.GetAllAsync())
                .ToList()
                .SelectMany(questionnaire => questionnaire.Questions.ToList())
                .FirstOrDefault(questionnaireQuestion => questionnaireQuestion.Id == questionId);
        }

        public Task<Answer> CreateAnswer(Answer answer)
        {
            throw new System.InvalidOperationException();
        }

        public List<Question> GetQuestionsFromQuestionnaire(int questionnaireId)
        {
            return _syncService.GetEntity(questionnaireId).Questions.ToList();
        }

        public Task<TAnswer> GetAnswer<TAnswer>(int id) where TAnswer : Answer
        {
            throw new System.NotImplementedException();
        }

        public Task<List<PlannedInspection>> GetPlannedInspections(int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new System.InvalidOperationException();
        }

        public int SaveChanges()
        {
            throw new System.InvalidOperationException();
        }

        public void Sync()
        {
        }
    }
}