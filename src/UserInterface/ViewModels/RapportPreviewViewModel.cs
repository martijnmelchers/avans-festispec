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
using System.Collections.ObjectModel;

namespace Festispec.UI.ViewModels
{
    public class RapportPreviewViewModel: ViewModelBase
    {
        private IQuestionService _questionService;
        private IQuestionnaireService _questionnaireService;
        public ObservableCollection<Control> Charts { get; set; }
        public Festival selectedFestival { get; set; }

        public RapportPreviewViewModel(IQuestionService questionService, IQuestionnaireService questionnaireService)
        {
            _questionService = questionService;
            _questionnaireService = questionnaireService;


            var questionaire = _questionnaireService.GetQuestionnaire(2);
            var questions = _questionnaireService.GetQuestionsFromQuestionnaire(questionaire.Id);
            Charts = new ObservableCollection<Control>();

            foreach (var question in questions)
            {
                // Answers
                var converter = new GraphSelectorFactory().GetConverter(question);


                if(converter == null)
                {
                    continue;
                }

                var chartValues = converter.TypeToChart();

                if (chartValues.Count < 1)
                    continue;


                if (question.GraphType == Models.GraphType.Line)
                {
                    var lineControl = new LineChartControl(chartValues);
                    lineControl.Height = 300;
                    lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    Charts.Add(lineControl);
                }
                if (question.GraphType == Models.GraphType.Pie)
                {
                    var lineControl = new PieChartControl(chartValues);
                    lineControl.Height = 300;
                    lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;

                    Charts.Add(lineControl);
                }
                else
                {
                    /* var lineControl = new ColumnChartControl(chartValues);
                    lineControl.Height = 300;
                    lineControl.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    Charts.Children.Add(lineControl); */
                }

                // Add textbox
            }
        }



    }
}
