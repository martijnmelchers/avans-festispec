using Festispec.Models;
using Festispec.Models.Questions;
using Festispec.Models.Answers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionService
    {
        Task<Questionnaire> GetQuestionaire(int id);
    }
}
