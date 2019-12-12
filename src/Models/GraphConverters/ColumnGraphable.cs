using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models.GraphConverters
{
    public class ColumnGraphable : IGraphable
    {
        public List<GraphableSeries> TypeToChart(Question question)
        {
            List<GraphableSeries> series = new List<GraphableSeries>();
            var plannedInspections = question.Answers.Select(x => x.PlannedInspection);
            
            foreach (var plannedInspection in plannedInspections)
            {
                GraphableSeries serie = new GraphableSeries();
                serie.Values = new ChartValues<int>();
                serie.Title = plannedInspection.EventTitle;

                var answer = question.Answers.FirstOrDefault(x => x.PlannedInspection.Id == plannedInspection.Id);

                var numAnswer = (NumericAnswer)answer;
                serie.Values.Add(numAnswer.IntAnswer);
                series.Add(serie);
            }

            return series;
        }
    }
}
