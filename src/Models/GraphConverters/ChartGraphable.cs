using Festispec.Models.Interfaces;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using LiveCharts.Wpf;
using LiveCharts;
using Festispec.Models.Questions;
using Festispec.Models.Answers;

namespace Festispec.Models.GraphConverters
{
    public class ChartGraphable : IGraphable
    {
        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart(List<Answer> answers)
        {
            throw new NotImplementedException();
        }
    }
}
