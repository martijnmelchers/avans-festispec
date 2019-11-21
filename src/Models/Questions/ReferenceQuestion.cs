namespace Festispec.Models.Questions
{
    public class ReferenceQuestion : Question
    {
        public Question Question { get; set; }

        public override GraphType GraphType => Question.GraphType;
    }
}