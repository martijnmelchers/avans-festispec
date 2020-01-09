using System.Collections.Generic;
using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Separator = LiveCharts.Wpf.Separator;

namespace Festispec.UI.Views.Controls
{
    public partial class LineChartControl
    {
        public LineChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();
            YFormatter = new AxesCollection();
            XFormatter = new AxesCollection();

            YFormatter.Add(new Axis
            {
                IsMerged = true,
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = true
                }
            });

            XFormatter.Add(new Axis
            {
                IsMerged = true,
                Separator = new Separator
                {
                    Step = 1,
                    IsEnabled = true
                }
            });

            foreach (GraphableSeries graphableSeries in values)
                SeriesCollection.Add(
                    new LineSeries
                    {
                        Title = graphableSeries.Title,
                        Values = graphableSeries.Values,
                        PointGeometry = null
                    });


            Chart.LegendLocation = LegendLocation.Bottom;
            DataContext = this;
        }

        public AxesCollection YFormatter { get; set; }
        public AxesCollection XFormatter { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
    }
}