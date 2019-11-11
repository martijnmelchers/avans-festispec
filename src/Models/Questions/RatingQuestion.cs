namespace Festispec.Models.Questions
{
    public class RatingQuestion : Question
    {
        public string LowRatingDescription { get; set; }

        public string HighRatingDescription { get; set; }

        public override GraphType GraphType => GraphType.Column;
    }
}