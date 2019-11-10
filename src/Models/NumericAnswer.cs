using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models
{
    public class NumericAnswer : Answer, IAnswer<int>
    {
        public int IntAnswer { get; set; }


        public int GetAnswer()
        {
            return IntAnswer;
        }
    }
}