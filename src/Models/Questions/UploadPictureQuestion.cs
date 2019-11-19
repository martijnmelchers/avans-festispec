namespace Festispec.Models.Questions
{
    public class UploadPictureQuestion : Question
    {
        public UploadPictureQuestion(string contents, Questionnaire questionnaire) : base(contents, questionnaire) { }
        public UploadPictureQuestion() : base() { }

        public override GraphType GraphType => GraphType.None;
    }
}