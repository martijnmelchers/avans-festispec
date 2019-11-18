using System;
using System.Collections.Generic;
using System.Text;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.Questions;
namespace Festispec.DomainServices.Services
{
    class QuestionService: IQuestionService
    {

        private static List<Question> Questions = new List<Question>()
        {
              new DrawQuestion(),
              new NumericQuestion()
        };

        public List<Question> GetQuestions()
        {
            return Questions;
        }
    }
}
