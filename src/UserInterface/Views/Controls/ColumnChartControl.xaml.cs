using System.Collections.Generic;
using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Festispec.UI.Views.Controls
{
    public partial class ColumnChartControl
    {
        public ColumnChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();

            foreach (GraphableSeries graphableSeries in values)
                SeriesCollection.Add(
                    new ColumnSeries
                    {
                        Title = graphableSeries.Title,
                        Values = graphableSeries.Values
                    });
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}