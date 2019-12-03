using Festispec.Models;
using Festispec.Models.Answers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionService
    {

        List<Answer> GetAnswers(Models.Questions.Question answer);
        Questionnaire GetQuestionaire(int id);

        Festival GetFestival(int id);
    }
}
