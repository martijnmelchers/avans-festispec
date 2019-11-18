using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.GraphConverters
{
    public class LineGraphable : IGraphable
    {
        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart(List<Answer> answers)
        {
            List<GraphableSeries> series = new List<GraphableSeries>();

            GraphableSeries serie = new GraphableSeries();
            serie.Title = answers[0].Question.Contents;


            ChartValues<int> values = new ChartValues<int>();
            foreach(var answer in answers)
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
