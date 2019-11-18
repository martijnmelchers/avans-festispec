using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.Interfaces
{
    public interface IGraphable
    {
        public List<GraphableSeries> TypeToChart();
    }
}
