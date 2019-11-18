using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Festispec.UI.Views.Controls
{
    /// <summary>
    /// Interaction logic for LineChartControl.xaml
    /// </summary>
    public partial class LineChartControl : UserControl
    {

        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public LineChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {

            };

            Labels = new[] { "Gay", "Boi", "Fuk", "Yuu"};
            YFormatter = value => value.ToString("C");

            foreach (var GraphableSeries in values)
                {
                    SeriesCollection.Add(
                     new LineSeries
                     {
                        Title = GraphableSeries.Title,
                        Values = GraphableSeries.Values
                     });
                }
            DataContext = this;
        }
    }
}