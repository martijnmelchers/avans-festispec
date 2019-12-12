using Festispec.Models;
using Festispec.Models.Answers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionService
    {

        Task<List<Answer>> GetAnswers(Models.Questions.Question answer);
        Task<Questionnaire> GetQuestionaire(int id);
    }
}
