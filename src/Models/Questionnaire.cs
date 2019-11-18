using Festispec.Models.Answers;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;

namespace Festispec.Models
{
    public class Questionnaire : Entity
    {
        public int Id { get; set; }

        public DateTime IsComplete { get; set; }

        public virtual PlannedInspection PlannedInspection { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public void GenerateReport()
        {
            foreach(var question in Questions)
            {
                var converter = new GraphSelectorFactory().GetConverter(question);
                var values = converter.TypeToChart();

                                
            }
        }
    }
}
