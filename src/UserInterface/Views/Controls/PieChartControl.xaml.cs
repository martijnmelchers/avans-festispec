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
    /// Interaction logic for PieChartControl.xaml
    /// </summary>
    public partial class PieChartControl : UserControl
    {

        public SeriesCollection SeriesCollection { get; set; }

        public PieChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {

            };


            foreach (var GraphableSeries in values)
            {
                SeriesCollection.Add(
                 new PieSeries
                 {
                     Title = GraphableSeries.Title,
                     Values = GraphableSeries.Values,
                     DataLabels = true
                 }); 
            }



            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }
    }
}
