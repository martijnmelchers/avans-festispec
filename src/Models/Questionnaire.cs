﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Festispec.Models.Questions;

namespace Festispec.Models
{
    public class Questionnaire : Entity
    {
        public Questionnaire(string name, Festival festival)
        {
            Name = name;
            Festival = festival;
            Questions = new List<Question>();
        }

        public Questionnaire()
        {
        }


        [Required]
        [MinLength(5)]
        [MaxLength(45)]
        public virtual string Name { get; set; }

        public int Id { get; set; }

        public DateTime? IsComplete { get; set; }

        [Required] public virtual Festival Festival { get; set; }

        public virtual ICollection<PlannedInspection> PlannedInspections { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}