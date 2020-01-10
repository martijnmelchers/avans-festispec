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

            ICollection<Answer> answers = question.Answers;
            var serie = new GraphableSeries();
            serie.Title = question.Contents;

            var chartValues = new ChartValues<float>();
            foreach (Answer answer in answers)
            {
                var numAnswer = (NumericAnswer) answer;
                chartValues.Add(numAnswer.IntAnswer);
            }

            serie.Values = chartValues;
            series.Add(serie);

            return series;
        }
    }
}