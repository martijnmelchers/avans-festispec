using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class NumericQuestion : Question
    {
        public NumericQuestion(string contents, Questionnaire questionnaire, int minimum, int maximum) : base(contents,
            questionnaire)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public NumericQuestion()
        {
        }

        [Required] public int Minimum { get; set; }

        [Required] public int Maximum { get; set; }

        // bijv. Meter, personen, etc.
        public AnswerUnit Unit { get; set; }

        public override GraphType GraphType => GraphType.Line;
    }
}