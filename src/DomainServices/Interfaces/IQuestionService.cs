using System.Threading.Tasks;
using Festispec.Models;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionService
    {
        Task<Questionnaire> GetQuestionaire(int id);
    }
}