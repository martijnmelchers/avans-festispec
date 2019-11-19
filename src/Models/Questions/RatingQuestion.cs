namespace Festispec.Models.Questions
{
    public class RatingQuestion : Question
    {
        public RatingQuestion(string contents, QuestionCategory category, Questionnaire questionnaire) : base(contents, category, questionnaire) { }
        public string LowRatingDescription { get; set; }

        public string HighRatingDescription { get; set; }

        public override GraphType GraphType => GraphType.Column;
    }
}