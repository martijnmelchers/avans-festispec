using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class RatingQuestion : Question
    {
        public RatingQuestion(string contents, Questionnaire questionnaire, string lowRatingDescription,
            string highRatingDescription) : base(contents, questionnaire)
        {
            LowRatingDescription = lowRatingDescription;
            HighRatingDescription = highRatingDescription;
        }

        public RatingQuestion()
        {
        }

        [Required] public string LowRatingDescription { get; set; }

        [Required] public string HighRatingDescription { get; set; }

        public override GraphType GraphType => GraphType.Column;
    }
}