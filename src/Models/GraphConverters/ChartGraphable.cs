using Festispec.Models.Interfaces;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts.Wpf;
using LiveCharts;
using Festispec.Models.Questions;
using Festispec.Models.Answers;

namespace Festispec.Models.GraphConverters
{
    public class ChartGraphable : IGraphable
    {
        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart(ICollection<Answer> answers)
        {

            List<GraphableSeries> series = new List<GraphableSeries>();

            GraphableSeries serie = new GraphableSeries();
            serie.Title = Question.Contents;

            ChartValues<int> values = new ChartValues<int>();
            foreach (var answer in answers)
            {
                var numAnswer = (NumericAnswer)answer;
                values.Add(numAnswer.IntAnswer);
            }

            serie.Values = values;
            series.Add(serie);
            return series;
        }
    }
}
