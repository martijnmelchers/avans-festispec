using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models
{
    public class MultipleChoiceAnswer : Answer, IAnswer<int>
    {
        public int MultipleChoiceAnswerKey { get; set; }

        public int GetAnswer()
        {
            return MultipleChoiceAnswerKey;
        }
    }
}