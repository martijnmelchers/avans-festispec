using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using System.Collections.Generic;

namespace Festispec.Models.Questions
{
    public class NumericQuestion : Question, IAnswerable<NumericAnswer>
    {
        public int Minimum { get; set; }

        public int Maximum { get; set; }

        // bijv. Meter, personen, etc.
        public AnswerUnit Unit { get; set; }

        public override GraphType GraphType => GraphType.Line;

        public new virtual ICollection<NumericAnswer> Answers { get; set; }

    }
}