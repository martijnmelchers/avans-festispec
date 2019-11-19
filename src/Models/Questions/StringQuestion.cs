namespace Festispec.Models.Questions
{
    public class StringQuestion : Question
    {
        public StringQuestion(string contents, QuestionCategory category, Questionnaire questionnaire) : base(contents, category, questionnaire) { }

        public const int CharacterLimit = 400;

        public bool IsMultiline { get; set; }

        public override GraphType GraphType => GraphType.None;
    }
}