namespace Festispec.Models.Questions
{
    public class NumericQuestion : Question
    {
        public int Minimum { get; set; }

        public int Maximum { get; set; }

        // bijv. Meter, personen, etc.
        public AnswerUnit Unit { get; set; }

        public override GraphType GraphType => GraphType.Line;
    }
}