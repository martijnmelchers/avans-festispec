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
        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart()
        {
            List<GraphableSeries> series = new List<GraphableSeries>();
            var plannedInspections = Question.Answers.Select(x => x.PlannedInspection);

            foreach (var plannedInspection in plannedInspections)
            {
                var answers = Question.Answers.Where(x => x.PlannedInspection == plannedInspection);


                GraphableSeries serie = new GraphableSeries();
                serie.Title = Question.Contents;


                var chartValues = new ChartValues<int>();
                foreach (var answer in answers)
                {
                    NumericAnswer numAnswer = (NumericAnswer)answer;
                    chartValues.Add(numAnswer.IntAnswer);
                }
                serie.Values = chartValues;
                series.Add(serie);
            }

            return series;
        }
    }
}
