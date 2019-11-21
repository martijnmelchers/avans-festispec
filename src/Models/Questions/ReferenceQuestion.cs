namespace Festispec.Models
{
    public class ReferenceQuestion : Questions.Question
    {
        public Questions.Question Question { get; set; }

        public override GraphType GraphType => Question.GraphType;
    }
}