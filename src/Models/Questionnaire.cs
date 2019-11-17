using Festispec.Models.Answers;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;

namespace Festispec.Models
{
    public class Questionnaire : Entity
    {
        public Questionnaire(string name, Festival festival)
        {
            Name = name;
            Festival = festival;
            Answers = new List<Answer>();
            Questions = new List<Question>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime IsComplete { get; set; }

        public virtual Festival Festival { get; set; }

        public virtual PlannedInspection PlannedInspection { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}