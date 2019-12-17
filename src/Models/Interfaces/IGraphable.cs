using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Festispec.Models.Answers;
using Festispec.Models.Questions;

namespace Festispec.Models.Interfaces
{
    public interface IGraphable
    {
        public List<GraphableSeries> TypeToChart(Question question);
    }
}
