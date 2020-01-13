using Festispec.Models.Interfaces;

namespace Festispec.Models.Answers
{
    public class MultipleChoiceAnswer : Answer
    {
        public int MultipleChoiceAnswerKey { get; set; }

        public int GetAnswer()
        {
            return MultipleChoiceAnswerKey;
        }
    }
}