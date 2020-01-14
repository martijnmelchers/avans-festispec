using System.Collections.Generic;
using System.Linq;
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
            var graphableSeries = new GraphableSeries { Title = question.Contents };

            var chartValues = new ChartValues<float>();
            foreach (var answer in question.Answers.OfType<NumericAnswer>())
                chartValues.Add(answer.IntAnswer);
            

            graphableSeries.Values = chartValues;
            series.Add(graphableSeries);

            return series;
        }
    }
}