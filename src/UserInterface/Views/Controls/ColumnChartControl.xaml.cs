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
    /// Interaction logic for ColumnChartControl.xaml
    /// </summary>
    public partial class ColumnChartControl : UserControl
    {

        public SeriesCollection SeriesCollection { get; set; }

        public ColumnChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();
            

            SeriesCollection = new SeriesCollection();


            foreach (var GraphableSeries in values)
            {
                SeriesCollection.Add(
                 new ColumnSeries
                 {
                     Title = GraphableSeries.Title,
                     Values = GraphableSeries.Values
                 });
            }
            DataContext = this;
        }
    }
}
