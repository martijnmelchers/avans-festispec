using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel
    {
        private IQuestionService questionService;
        public Festival selectedFestival { get; set; }
        public RapportPreviewViewModel(IQuestionService questionService)
        {
            this.questionService = questionService;
            questionService.GetFestival(1);
        }



    }
}
