using System.Collections.Generic;
using System.Windows.Controls;
using Festispec.Models;
using LiveCharts;
using LiveCharts.Wpf;

namespace Festispec.UI.Views.Controls
{
    /// <summary>
    ///     Interaction logic for ColumnChartControl.xaml
    /// </summary>
    public partial class ColumnChartControl : UserControl
    {
        public ColumnChartControl(List<GraphableSeries> values)
        {
            InitializeComponent();


            SeriesCollection = new SeriesCollection();


            foreach (GraphableSeries GraphableSeries in values)
                SeriesCollection.Add(
                    new ColumnSeries
                    {
                        Title = GraphableSeries.Title,
                        Values = GraphableSeries.Values
                    });
            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}