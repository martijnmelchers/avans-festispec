using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Festispec.Models.Answers;
using Festispec.Models.Questions;

namespace Festispec.DomainServices.Factories
{
    [ExcludeFromCodeCoverage]
    public class AnswerFactory
    {
        public Answer GetAnswer(Question question)
        {
            return question switch
            {
                NumericQuestion _ => new NumericAnswer(),
                RatingQuestion _ => new NumericAnswer(),
                MultipleChoiceQuestion _ => new MultipleChoiceAnswer(),
                StringQuestion _ => new StringAnswer(),
                DrawQuestion _ => new FileAnswer(),
                UploadPictureQuestion _ => new FileAnswer(),
                _ => throw new Exception()
            };

        }
    }
}