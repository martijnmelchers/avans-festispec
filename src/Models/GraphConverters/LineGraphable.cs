using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using LiveCharts;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.Models.GraphConverters
{
    public class LineGraphable : IGraphable
    {
        public Question Question { get; set; }

        public List<GraphableSeries> TypeToChart()
        {
            throw new NotImplementedException();
        }
    }
}
