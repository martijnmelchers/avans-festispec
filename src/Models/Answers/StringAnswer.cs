using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Answers
{
    public class StringAnswer : Answer, IAnswer<string>
    {
        public string AnswerContents { get; set; }

        public string GetAnswer()
        {
            return AnswerContents;
        }
    }
}