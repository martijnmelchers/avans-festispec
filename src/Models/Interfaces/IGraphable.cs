using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Festispec.Models.Answers;
namespace Festispec.Models.Interfaces
{
    public interface IGraphable
    {
        public Questions.Question Question { get; set; }
        public List<GraphableSeries> TypeToChart(ICollection<Answer> answers);
    }
}
