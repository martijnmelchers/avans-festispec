using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using System.Collections.Generic;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question, IAnswerable<MultipleChoiceAnswer>
    {
        public virtual ICollection<string> Options { get; set; }
        public override GraphType GraphType => GraphType.Pie;
        public new virtual ICollection<MultipleChoiceAnswer> Answers { get; set; }
    }
}