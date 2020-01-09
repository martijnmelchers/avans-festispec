using Festispec.Models;
using Festispec.Models.Answers;
using Festispec.Models.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionnaireService : ISaveable, ISyncable
    {
        Questionnaire GetQuestionnaire(int questionnaireId);
        Task<Questionnaire> CreateQuestionnaire(string name, Festival festival);
        Task<Question> AddQuestion(int questionnaireId, Question question);
        Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId);
        Task<bool> RemoveQuestion(int questionId);
        Task RemoveQuestionnaire(int questionnaireId);
        Task<Questionnaire> CopyQuestionnaire(int questionnaireId);
        Task<Question> GetQuestion(int questionId);
        Task<Answer> CreateAnswer(Answer answer);
        void Save();

        List<Question> GetQuestionsFromQuestionnaire(int questionnaireId);
        List<Answer> GetAnswers();
    }
}
