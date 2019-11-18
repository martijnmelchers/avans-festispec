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


            // Get questions/values
            // Use factory to generate each chart from values.
            // Add the chart to the current view.
            // Generate images for each chart.
            // Render pdf

            _scope = AppServices.Instance.ServiceProvider.CreateScope();

            var service = _scope.ServiceProvider.GetService<IQuestionService>();
            var questions = service.GetQuestions();



        }
    }
}
