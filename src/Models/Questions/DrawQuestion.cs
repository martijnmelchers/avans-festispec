using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class DrawQuestion : StringQuestion
    {
        public DrawQuestion(string contents, QuestionCategory category, Questionnaire questionnaire, string picturePath) : base(contents, category, questionnaire) 
        {
            PicturePath = picturePath;
        }
        public DrawQuestion() : base() { }

        [Required]
        public string PicturePath { get; set; }
    }
}