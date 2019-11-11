using Festispec.Models.Interfaces;

namespace Festispec.Models.Answers
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