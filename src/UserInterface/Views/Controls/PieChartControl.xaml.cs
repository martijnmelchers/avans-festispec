using System.Collections.Generic;
using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Festispec.UI.Views.Controls
{
    public partial class PieChartControl
    {
        public PieChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();


            foreach (var graphableSeries in values)
                SeriesCollection.Add(
                    new PieSeries
                    {
                        Title = graphableSeries.Title,
                        Values = graphableSeries.Values,
                        DataLabels = true
                    });


            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (PieChart) chartPoint.ChartView;

            //clear selected slice.
            foreach (var seriesView in chart.Series)
            {
                var series = (PieSeries) seriesView;
                series.PushOut = 0;
            }

            var selectedSeries = (PieSeries) chartPoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}