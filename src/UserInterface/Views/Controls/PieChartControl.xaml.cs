using System;
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


            foreach (GraphableSeries graphableSeries in values)
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

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (PieChart) chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries) chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}