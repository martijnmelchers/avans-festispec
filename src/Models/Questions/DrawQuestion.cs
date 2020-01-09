using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class DrawQuestion : UploadPictureQuestion
    {
        public DrawQuestion(string contents, Questionnaire questionnaire, string picturePath) : base(contents,
            questionnaire)
        {
            PicturePath = picturePath;
        }

        public DrawQuestion()
        {
        }

        [Required] public string PicturePath { get; set; }
    }
}