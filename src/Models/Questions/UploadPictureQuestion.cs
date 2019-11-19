namespace Festispec.Models.Questions
{
    public class UploadPictureQuestion : Question
    {
        public UploadPictureQuestion(string contents, QuestionCategory category, Questionnaire questionnaire) : base(contents, category, questionnaire) { }
        public UploadPictureQuestion() : base() { }

        public override GraphType GraphType => GraphType.None;
    }
}