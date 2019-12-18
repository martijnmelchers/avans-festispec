using Festispec.Models.Interfaces;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts.Wpf;
using LiveCharts;
using Festispec.Models.Questions;
using Festispec.Models.Answers;
using System.Linq;

namespace Festispec.Models.GraphConverters
{
    public class ChartGraphable : IGraphable
    {

        public List<GraphableSeries> TypeToChart(Question question)
        {

            var multipleChoiceAnswers = question.Answers ;

            List<GraphableSeries> chartSeries = new List<GraphableSeries>();


            if(multipleChoiceAnswers == null)
                return chartSeries;


            MultipleChoiceQuestion quest = (MultipleChoiceQuestion)question;

            for (var i = 0; i < quest.OptionCollection.Count; i++)
            {
                var option = quest.OptionCollection[i];
                // Hoevaak hebben we de index answered.

                var count = quest.Answers.Count(a => {
                    var answer = (MultipleChoiceAnswer)a; ;
                    return answer.MultipleChoiceAnswerKey == i;
                });

                var serie = new GraphableSeries()
                {
                    Title = option.Value,
                    Values = new ChartValues<int> { count }
                };
            
                chartSeries.Add(serie);
            }
            

            return chartSeries;
        }
    }
}