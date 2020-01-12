using System.Collections.Generic;
using System.Threading.Tasks;
using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionnaireService : ISaveable, ISyncable
    {
        Questionnaire GetQuestionnaire(int questionnaireId);
        Task<Questionnaire> CreateQuestionnaire(string name, int festivalId);
        Task<Question> AddQuestion(int questionnaireId, Question question);
        Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId);
        Task<bool> RemoveQuestion(int questionId);
        Task RemoveQuestionnaire(int questionnaireId);
        Task<Questionnaire> CopyQuestionnaire(int questionnaireId, string questionnaireName);
        Task<Question> GetQuestion(int questionId);
        Task<Answer> CreateAnswer(Answer answer);
        void Save();

        List<Question> GetQuestionsFromQuestionnaire(int questionnaireId);
        List<Answer> GetAnswers();

        Task<Answer> GetAnswer(int id);
        Task<TAnswer> GetAnswer<TAnswer>(int id) where TAnswer : Answer;


        Task<List<PlannedInspection>> GetPlannedInspections(int employeeId);
        Task<PlannedInspection> GetPlannedInspection(int plannedInspectionId);
    }
}