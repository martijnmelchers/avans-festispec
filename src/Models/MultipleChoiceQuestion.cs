using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class MultipleChoiceQuestion : Question
    {
        public virtual ICollection<string> Options { get; set; }
        public override GraphType GraphType => GraphType.Pie;
    }
}