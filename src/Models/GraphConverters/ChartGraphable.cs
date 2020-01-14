using System.Collections.Generic;
using System.Linq;
using Festispec.Models.Answers;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;

namespace Festispec.Models.GraphConverters
{
    public class ChartGraphable : IGraphable
    {
        public List<GraphableSeries> TypeToChart(Question question)
        {
            var chartSeries = new List<GraphableSeries>();
            var multipleChoiceQuestion = (MultipleChoiceQuestion) question;

            for (var i = 0; i < multipleChoiceQuestion.OptionCollection.Count; i++)
            {
                var option = multipleChoiceQuestion.OptionCollection[i];
                // Hoevaak hebben we de index answered.

                var count = multipleChoiceQuestion.Answers
                    .OfType<MultipleChoiceAnswer>()
                    .Count(a => a.MultipleChoiceAnswerKey == i);

                var graphableSeries = new GraphableSeries
                {
                    Title = option.Value,
                    Values = new ChartValues<int> { count }
                };

                chartSeries.Add(graphableSeries);
            }


            return chartSeries;
        }
    }
}