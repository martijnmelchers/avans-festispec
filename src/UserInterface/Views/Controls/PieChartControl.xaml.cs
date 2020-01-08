using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Festispec.UI.Views.Controls
{
    /// <summary>
    ///     Interaction logic for PieChartControl.xaml
    /// </summary>
    public partial class PieChartControl : UserControl
    {
        public PieChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection();


            foreach (GraphableSeries GraphableSeries in values)
                SeriesCollection.Add(
                    new PieSeries
                    {
                        Title = GraphableSeries.Title,
                        Values = GraphableSeries.Values,
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