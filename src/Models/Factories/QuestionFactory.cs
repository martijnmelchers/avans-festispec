using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Factories
{
    public class QuestionFactory
    {
        public IEnumerable<string> QuestionTypes => _questions.Keys;

        private readonly Dictionary<string, Question> _questions;
        public QuestionFactory()
        {
            _questions = new Dictionary<string, Question>
            {
                ["Tekenvraag"] = new DrawQuestion(),
                ["Open vraag"] = new StringQuestion(),
                ["Afbeeldingsvraag"] = new UploadPictureQuestion(),
                ["Beoordelingsvraag"] = new RatingQuestion(),
                ["Meerkeuzevraag"] = new MultipleChoiceQuestion(),
                ["Numerieke vraag"] = new NumericQuestion()
            };
        }
        public Question GetQuestionType(string type)
        {
            return _questions[type];
        }
    }
}
