﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Festispec.Models.Questions
{
    public class ReferenceQuestion : Questions.Question
    {
        public ReferenceQuestion(string contents, Questionnaire questionnaire, Question question) : base(contents, questionnaire) 
        {
            Question = question;
        }
        public ReferenceQuestion() : base() { }

        [Required]
        public Question Question { get; set; }

        public override GraphType GraphType => Question.GraphType;
    }
}