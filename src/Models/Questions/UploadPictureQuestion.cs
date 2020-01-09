namespace Festispec.Models.Questions
{
    public class UploadPictureQuestion : Question
    {
        public UploadPictureQuestion(string contents, Questionnaire questionnaire) : base(contents, questionnaire)
        {
        }

        public UploadPictureQuestion()
        {
        }

        public override GraphType GraphType => GraphType.None;
    }
}