using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Festispec.Models.GraphConverters
{
    public class LineGraphable : IGraphable
    {

        public List<GraphableSeries> TypeToChart(Question question)
        {
            List<GraphableSeries> series = new List<GraphableSeries>();
            var plannedInspections = question.Answers.Select(x => x.PlannedInspection);

            var answers = question.Answers;
            GraphableSeries serie = new GraphableSeries();
            serie.Title = question.Contents;

            var chartValues = new ChartValues<float>();
            foreach (var answer in answers)
            {
                NumericAnswer numAnswer = (NumericAnswer)answer;
                chartValues.Add(numAnswer.IntAnswer);
            }
            serie.Values = chartValues;
            series.Add(serie);
           
            return series;
        }
    }
}
