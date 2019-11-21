using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class NumericQuestion : Question, IAnswerable<NumericAnswer>
    {
        public NumericQuestion(string contents, Questionnaire questionnaire, int minimum, int maximum) : base(contents, questionnaire) 
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

        public new virtual ICollection<NumericAnswer> Answers { get; set; }

    }
}