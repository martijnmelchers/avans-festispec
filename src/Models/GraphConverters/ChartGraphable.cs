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


        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart()
        {

            var multipleChoiceAnswers = Question.Answers ;

            List<GraphableSeries> chartSeries = new List<GraphableSeries>();




            if(multipleChoiceAnswers == null) {
                return chartSeries;
            }


            MultipleChoiceQuestion quest = (MultipleChoiceQuestion)Question;

            for(var i = 0; i < quest.OptionCollection.Count; i++)
            {
                var option = quest.OptionCollection[i];

                var serie = new GraphableSeries();
                serie.Title = option.Value;

                // Hoevaak hebben we de index answered.

                var count = quest.Answers.Count(x => {

                    var answerMC = (MultipleChoiceAnswer)x;
                    return answerMC.MultipleChoiceAnswerKey == i;
                });

                serie.Values = new ChartValues<int> { count };
                chartSeries.Add(serie);
            }
            
       

            return chartSeries;
        }
    }
}
