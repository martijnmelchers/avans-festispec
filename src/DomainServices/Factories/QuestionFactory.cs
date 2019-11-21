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
                ["Numerieke vraag"] = new NumericQuestion(),
                ["Referentievraag"] = new ReferenceQuestion()
            };
        }
        public Question GetQuestionType(string type)
        {
            switch (type)
            {
                case "Tekenvraag":
                    return new DrawQuestion();
                case "Open vraag":
                    return new StringQuestion();
                case "Afbeeldingsvraag":
                    return new UploadPictureQuestion();
                case "Beoordelingsvraag":
                    return new RatingQuestion();
                case "Meerkeuzevraag":
                    return new MultipleChoiceQuestion();
                case "Numerieke vraag":
                    return new NumericQuestion();
                case "Referentievraag":
                    return new ReferenceQuestion();
            }
            return null;
        }
    }
}
