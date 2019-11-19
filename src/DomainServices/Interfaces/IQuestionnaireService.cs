using Festispec.Models;
using Festispec.Models.Questions;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionnaireService
    {
        Questionnaire GetQuestionnaire(int questionnaireId);
        Task<Questionnaire> CreateQuestionnaire(string name, Festival festival);
        Task AddQuestion(Questionnaire questionnaire, Question question);
        Task RemoveQuestion(Questionnaire questionnaire, int questionId);
        Task RemoveQuestionnaire(int questionnaireId);
        Task CopyQuestionnaire(int questionnaireId);
    }
}
