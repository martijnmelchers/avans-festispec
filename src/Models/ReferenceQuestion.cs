using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models
{
    public class ReferenceQuestion : Questions.Question
    {
        public Questions.Question Question { get; set; }

        public override GraphType GraphType => Question.GraphType;
    }
}