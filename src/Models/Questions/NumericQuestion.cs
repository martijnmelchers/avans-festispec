namespace Festispec.Models.Questions
{
    public class NumericQuestion : Question
    {
        public NumericQuestion(string contents, QuestionCategory category, Questionnaire questionnaire) : base(contents, category, questionnaire) { }
        public int Minimum { get; set; }

        public int Maximum { get; set; }

        // bijv. Meter, personen, etc.
        public AnswerUnit Unit { get; set; }

        public override GraphType GraphType => GraphType.Line;
    }
}