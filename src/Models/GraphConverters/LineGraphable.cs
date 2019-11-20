using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;
using System.Collections.Generic;

namespace Festispec.Models.GraphConverters
{
    public class LineGraphable : IGraphable
    {

        public List<GraphableSeries> ToChart<TQuestion>(TQuestion question) where TQuestion : Question
        {
            List<GraphableSeries> series = new List<GraphableSeries>();

            GraphableSeries serie = new GraphableSeries
            {
                Title = question.Contents
            };


            ChartValues<int> values = new ChartValues<int>();
            foreach(NumericAnswer answer in question.Answers)
                values.Add(answer.IntAnswer);

            serie.Values = values;
            series.Add(serie);
            return series;
        }
    }
}
