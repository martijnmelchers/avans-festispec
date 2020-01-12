using System.Collections.Generic;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;

namespace Festispec.Models.GraphConverters
{
    public class LineGraphable : IGraphable
    {
        public List<GraphableSeries> TypeToChart(Question question)
        {
            var series = new List<GraphableSeries>();

            var answers = question.Answers;
            var graphableSeries = new GraphableSeries { Title = question.Contents };

            var chartValues = new ChartValues<float>();
            foreach (Answer answer in answers)
            {
                var numAnswer = (NumericAnswer) answer;
                chartValues.Add(numAnswer.IntAnswer);
            }

            graphableSeries.Values = chartValues;
            series.Add(graphableSeries);

            return series;
        }
    }
}