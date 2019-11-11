using Festispec.Models.Interfaces;

namespace Festispec.Models.Answers
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