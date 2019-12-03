using Festispec.UI.Views.Controls;
using LiveCharts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Festispec.DomainServices.Interfaces;
using Festispec.Models.Questions;

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for RenderWindow.xaml
    /// </summary>
    public partial class RenderWindow : Window
    {

        private readonly IServiceScope _scope;

        public RenderWindow()
        {
            InitializeComponent();


            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            var service = _scope.ServiceProvider.GetService<IQuestionService>();
            var questionaire = service.GetQuestionaire(2);
            var questions = questionaire.Questions;

            foreach(var question in questions)
            {
                // Answers
                var answers = question.Answers;
                var converter = new GraphSelectorFactory().GetConverter(question);
                var chartValues = converter.TypeToChart(answers);


                if(question.GraphType == Models.GraphType.Line)
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
