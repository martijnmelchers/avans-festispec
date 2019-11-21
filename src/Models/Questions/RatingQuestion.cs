using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public class RatingQuestion : Question, IAnswerable<NumericAnswer>
    {
        public RatingQuestion(string contents, Questionnaire questionnaire, string lowRatingDescription, string highRatingDescription) : base(contents, questionnaire) 
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

        public new virtual ICollection<NumericAnswer> Answers { get; set; }
    }
}