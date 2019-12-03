using Festispec.Models;
using Festispec.Models.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionnaireService
    {
        Questionnaire GetQuestionnaire(int questionnaireId);
        Task<Questionnaire> CreateQuestionnaire(string name, Festival festival);
        Task<Question> AddQuestion(int questionnaireId, Question question);
        Question GetQuestionFromQuestionnaire(int questionnaireId, int questionId);
        Task<bool> RemoveQuestion(int questionId);
        Task RemoveQuestionnaire(int questionnaireId);
        Task<Questionnaire> CopyQuestionnaire(int questionnaireId);

        List<Question> GetQuestionsFromQuestionnaire(int questionnaireId);

    }
}
