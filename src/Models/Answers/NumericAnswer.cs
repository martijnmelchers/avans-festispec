using Festispec.Models.Interfaces;

namespace Festispec.Models.Answers
{
    public class NumericAnswer : Answer
    {
        public int IntAnswer { get; set; }


        public int GetAnswer()
        {
            return IntAnswer;
        }
    }
}