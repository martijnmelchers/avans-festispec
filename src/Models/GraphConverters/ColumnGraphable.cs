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
            var plannedInspections = question.Answers.Select(x => x.PlannedInspection);

            foreach (var plannedInspection in plannedInspections)
            {
                var graphableSeries = new GraphableSeries
                {
                    Title = plannedInspection.EventTitle,
                    Values = new ChartValues<int>()
                };

                var answer = question.Answers
                    .OfType<NumericAnswer>()
                    .FirstOrDefault(x => x.PlannedInspection.Id == plannedInspection.Id);

                if (answer != null) graphableSeries.Values.Add(answer.IntAnswer);
                series.Add(graphableSeries);
            }

            return series;
        }
    }
}