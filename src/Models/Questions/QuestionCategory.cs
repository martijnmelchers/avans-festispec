using Festispec.Models.Interfaces;
using System.Collections.Generic;

namespace Festispec.Models.Questions
{
    public class QuestionCategory : Entity
    {
        public int Id { get; set; }
       
        public string CategoryName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}