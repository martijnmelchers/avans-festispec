using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class RatingQuestion : Question
    {
        public RatingQuestion(string contents, QuestionCategory category, Questionnaire questionnaire, string lowRatingDescription, string highRatingDescription) : base(contents, category, questionnaire) 
        {
            LowRatingDescription = lowRatingDescription;
            HighRatingDescription = highRatingDescription;
        }
        public RatingQuestion() : base() { }

        [Required]
        public string LowRatingDescription { get; set; }

        [Required]
        public string HighRatingDescription { get; set; }

        public override GraphType GraphType => GraphType.Column;
    }
}