using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Exception;
using Festispec.Models.Questions;

namespace Festispec.DomainServices.Services
{
    public class OfflineQuestionnaireService : IQuestionnaireService
    {
        private readonly SyncService<Questionnaire> _syncService;

        public OfflineQuestionnaireService(SyncService<Questionnaire> syncService)
        {
            _syncService = syncService;
        }
        
        public Questionnaire GetQuestionnaire(int questionnaireId)
        {
            return _syncService.GetEntity(questionnaireId);
        }

        public Task<Questionnaire> CreateQuestionnaire(string name, Festival festival)
        {
            throw new System.InvalidOperationException();
        }

        public Task<Question> AddQuestion(int questionnaireId, Question question)
        {
            throw new System.InvalidOperationException();
        }

        public Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId)
        {
            Question questionFromQuestionnaire = _syncService.GetEntity(questionnaireId).Questions.FirstOrDefault(q => q.Id == questionId);
            
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

        public Task<Questionnaire> CopyQuestionnaire(int questionnaireId)
        {
            throw new System.InvalidOperationException();
        }

        public List<Question> GetQuestionsFromQuestionnaire(int questionnaireId)
        {
            return _syncService.GetEntity(questionnaireId).Questions.ToList();
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