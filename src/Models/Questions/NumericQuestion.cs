using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class NumericQuestion : Question
    {
        public NumericQuestion(string contents, QuestionCategory category, Questionnaire questionnaire, int minimum, int maximum) : base(contents, category, questionnaire) 
        {
            Minimum = minimum;
            Maximum = maximum;
        }
        public NumericQuestion() : base() { }

        [Required]
        public int Minimum { get; set; }

        [Required]
        public int Maximum { get; set; }

        // bijv. Meter, personen, etc.
        public AnswerUnit Unit { get; set; }

        public override GraphType GraphType => GraphType.Line;
    }
}