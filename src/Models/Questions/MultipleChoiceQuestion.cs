using System.Collections.Generic;

namespace Festispec.Models.Questions
{
    public class MultipleChoiceQuestion : Question
    {
        public virtual ICollection<string> Options { get; set; }
        public override GraphType GraphType => GraphType.Pie;
    }
}