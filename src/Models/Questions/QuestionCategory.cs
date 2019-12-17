using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Festispec.Models.Questions
{
    public class QuestionCategory : Entity
    {
        public int Id { get; set; }
       
        [Required, MaxLength(45)]
        public string CategoryName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}