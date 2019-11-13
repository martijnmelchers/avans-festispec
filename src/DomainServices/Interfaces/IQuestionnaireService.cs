using Festispec.Models;
using Festispec.Models.Questions;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionnaireService
    {
        Task<Questionnaire> CreateQuestionnaire(PlannedInspection plannedInspection);
        Task AddQuestion(Questionnaire questionnaire, Question question);
        Task RemoveQuestion(Questionnaire questionnaire, int questionId);
        Task EditQuestion(Questionnaire questionnaire, Question question);
    }
}
