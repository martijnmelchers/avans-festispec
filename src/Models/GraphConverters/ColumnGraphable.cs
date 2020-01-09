using System.Collections.Generic;
using System.Linq;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;

namespace Festispec.Models.GraphConverters
{
    public class ColumnGraphable : IGraphable
    {
        public List<GraphableSeries> TypeToChart(Question question)
        {
            var series = new List<GraphableSeries>();
            IEnumerable<PlannedInspection> plannedInspections = question.Answers.Select(x => x.PlannedInspection);

            foreach (PlannedInspection plannedInspection in plannedInspections)
            {
                var serie = new GraphableSeries
                {
                    Title = plannedInspection.EventTitle,
                    Values = new ChartValues<int>()
                };

                Answer answer = question.Answers.FirstOrDefault(x => x.PlannedInspection.Id == plannedInspection.Id);

                var numAnswer = (NumericAnswer) answer;
                serie.Values.Add(numAnswer.IntAnswer);
                series.Add(serie);
            }

            return series;
        }
    }
}