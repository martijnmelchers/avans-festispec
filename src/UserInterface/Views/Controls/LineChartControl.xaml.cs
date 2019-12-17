using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;

using System.Windows.Controls;


namespace Festispec.UI.Views.Controls
{
    /// <summary>
    /// Interaction logic for LineChartControl.xaml
    /// </summary>
    public partial class LineChartControl : UserControl
    {

        public string[] Labels { get; set; }
        public AxesCollection YFormatter { get; set; }
        public AxesCollection XFormatter { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
        public LineChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();
            YFormatter = new AxesCollection();
            XFormatter = new AxesCollection();
            YFormatter.Add(new Axis
            {
                IsMerged = true,
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 1,
                    IsEnabled = true
                }
            });

            XFormatter.Add(new Axis
            {
                IsMerged = true,
                Separator = new LiveCharts.Wpf.Separator
                {
                    Step = 1,
                    IsEnabled = true
                }
            });

            foreach (var GraphableSeries in values)
                {
                    SeriesCollection.Add(
                     new LineSeries
                     {
                        Title = GraphableSeries.Title,
                        Values = GraphableSeries.Values,
                        PointGeometry = null,
                        
                     });
                }


            Chart.LegendLocation = LegendLocation.Bottom;
            DataContext = this;
        }
    }
}
