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
            ICollection<Answer> multipleChoiceAnswers = question.Answers;

            var chartSeries = new List<GraphableSeries>();


            if (multipleChoiceAnswers == null)
                return chartSeries;


            var quest = (MultipleChoiceQuestion) question;

            for (var i = 0; i < quest.OptionCollection.Count; i++)
            {
                StringObject option = quest.OptionCollection[i];
                // Hoevaak hebben we de index answered.

                int count = quest.Answers.Count(a =>
                {
                    var answer = (MultipleChoiceAnswer) a;
                    return answer.MultipleChoiceAnswerKey == i;
                });

                var serie = new GraphableSeries
                {
                    Title = option.Value,
                    Values = new ChartValues<int> {count}
                };

                chartSeries.Add(serie);
            }


            return chartSeries;
        }
    }
}