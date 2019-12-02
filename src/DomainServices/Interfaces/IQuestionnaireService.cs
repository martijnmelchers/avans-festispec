using Festispec.Models;
using Festispec.Models.Questions;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionnaireService
    {
        Questionnaire GetQuestionnaire(int questionnaireId);
        Task<Questionnaire> CreateQuestionnaire(string name, Festival festival);
        Task<Question> AddQuestion(Questionnaire questionnaire, Question question);
        Question GetQuestionFromQuestionnaire(Questionnaire questionnaire, int questionId);
        Task<bool> RemoveQuestion(int questionId);
        Task RemoveQuestionnaire(int questionnaireId);
        Task<Questionnaire> CopyQuestionnaire(int questionnaireId);
    }
}
