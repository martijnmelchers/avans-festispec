using Festispec.DomainServices.Interfaces;
using Festispec.Models;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Festispec.UI.Views.Controls;
using GalaSoft.MvvmLight;
using System.Windows.Controls;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel: ViewModelBase
    {
        private IQuestionService questionService;
        public Festival selectedFestival { get; set; }
        public StackPanel Charts { get; set; }
        public RapportPreviewViewModel(IQuestionService questionService)
        {
            this.questionService = questionService;
            questionService.GetFestival(1);
            var questionaire = this.questionService.GetQuestionaire(2);
            var questions = questionaire.Questions;

            foreach (var question in questions)
            {
                // Answers
                var answers = question.Answers;
                var converter = new GraphSelectorFactory().GetConverter(question);
                var chartValues = converter.TypeToChart(answers);


                if (question.GraphType == Models.GraphType.Line)
                {
                    var lineControl = new LineChartControl(chartValues);
                    lineControl.Height = 300;
                    lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    Charts.Children.Add(lineControl);
                }
                else
                {
                    /* var lineControl = new ColumnChartControl(chartValues);
                    lineControl.Height = 300;
                    lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    Charts.Children.Add(lineControl); */
                }
            }
        }



    }
}
