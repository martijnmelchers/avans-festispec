using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public abstract class Question : Entity, IAnswerable<Answer>
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Contents { get; set; }

        [Required]
        public virtual QuestionCategory Category { get; set; }

        [Required]
        public virtual Questionnaire Questionnaire { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public abstract GraphType GraphType { get; }
    }
}