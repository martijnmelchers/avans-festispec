using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Interfaces
{
    public interface IGraphable
    {
        public Questions.Question Question { get; set; }
        public List<GraphableSeries> TypeToChart();
    }
}
