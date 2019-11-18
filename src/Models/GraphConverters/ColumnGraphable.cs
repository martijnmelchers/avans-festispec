using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.GraphConverters
{
    public class ColumnGraphable : IGraphable
    {
        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart(List<Answer> answers)
        {



            var values = new ChartValues<ObservableValue> {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8)
            };

            var graphvalues = new GraphableSeries { Title = "Epic", Values = values };
            var list = new List<GraphableSeries>() { graphvalues};

            return list;
        }
    }
}
