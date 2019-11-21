using Festispec.Models.Interfaces;

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