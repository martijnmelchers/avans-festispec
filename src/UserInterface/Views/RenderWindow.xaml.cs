using Festispec.UI.Views.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Festispec.DomainServices.Interfaces;
using System.Linq;
using Festispec.Models;
using Festispec.DomainServices.Factories;

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
            var questionaire = service.GetQuestionaire(1);
            var questions = service.GetQuestions(questionaire);

            foreach(var question in questions.Where(q => q.GraphType != GraphType.None))
            {
                // Answers
                var answers = service.GetAnswers(question);
                var converter = new GraphableFactory().GetConverter(question.GraphType);
                var chartValues = converter.ToChart(question);



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
