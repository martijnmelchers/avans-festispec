using System.Collections.Generic;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public class RatingQuestion : Question, IAnswerable<NumericAnswer>
    {
        public string LowRatingDescription { get; set; }

        public string HighRatingDescription { get; set; }

        public override GraphType GraphType => GraphType.Column;

        public new virtual ICollection<NumericAnswer> Answers { get; set; }
    }
}