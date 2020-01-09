namespace Festispec.Models.Questions
{
    public class StringQuestion : Question
    {
        public const int CharacterLimit = 400;

        public StringQuestion(string contents, Questionnaire questionnaire) : base(contents, questionnaire)
        {
        }

        public StringQuestion()
        {
        }

        public bool IsMultiline { get; set; }

        public override GraphType GraphType => GraphType.None;
    }
}