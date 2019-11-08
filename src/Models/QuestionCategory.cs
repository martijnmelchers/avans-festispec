using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Festispec.Models
{
    public class QuestionCategory : Entity
    {
        public int Id { get; set; }
       
        public string CategoryName { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}