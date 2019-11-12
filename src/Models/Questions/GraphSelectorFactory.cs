using System;
using System.Collections.Generic;
using System.Text;
using Festispec.Models;
using Festispec.Models.GraphConverters;
using Festispec.Models.Interfaces;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;

namespace Festispec.Models.Questions
{
    public class GraphSelectorFactory
    {
        private Dictionary<GraphType, IGraphable> _converters;

        public GraphSelectorFactory()
        {
            _converters = new Dictionary<GraphType, IGraphable>();
            _converters[GraphType.Pie] = new ChartGraphable();
            _converters[GraphType.Line] = new LineGraphable();
            _converters[GraphType.Column] = new ColumnGraphable();
        }


        public IGraphable GetConverter(Question question)
        {
            var graphType = question.GraphType;

            // Get answers where question = question.id.

            // (Type answer)



            // Check number of answers > 1

            // Loop door answers en stop ze in een series / meerdere series.

            // Creëer chart, return

            return _converters[question.GraphType];
        }
    }
}
