using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.DomainServices.Interfaces
{
    public interface IQuestionService
    {
        List<Models.Questions.Question> GetQuestions();
    }
}
