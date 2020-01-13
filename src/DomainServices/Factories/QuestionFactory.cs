using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Festispec.Models.Questions;

namespace Festispec.DomainServices.Factories
{
    [ExcludeFromCodeCoverage]
    public class QuestionFactory
    {
        public QuestionFactory()
        {
            QuestionTypes = new List<string>
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

        public IEnumerable<string> QuestionTypes { get; }

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