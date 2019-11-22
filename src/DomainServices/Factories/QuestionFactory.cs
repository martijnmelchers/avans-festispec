using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Factories
{
    public class QuestionFactory
    {
        public IEnumerable<string> QuestionTypes { get; }

        public QuestionFactory()
        {
            QuestionTypes = new List<String>()
            {
                "Tekenvraag",
                "Open vraag",
                "Afbeeldingsvraag",
                "Beoordelingsvraag",
                "Meerkeuzevraag",
                "Numerieke vraag",
                "Referentievraag"
            };

        }
        public Question GetQuestionType(string type)
        {
            return type switch
            {
                "Tekenvraag" => new DrawQuestion(),
                "Open vraag" => new StringQuestion(),
                "Afbeeldingsvraag" => new UploadPictureQuestion(),
                "Beoordelingsvraag" => new RatingQuestion(),
                "Meerkeuzevraag" => new MultipleChoiceQuestion(),
                "Numerieke vraag" => new NumericQuestion(),
                "Referentievraag" => new ReferenceQuestion(),
                _ => null
            };
        }
    }
}
