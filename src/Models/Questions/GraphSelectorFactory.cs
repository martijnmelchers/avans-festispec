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
            _converters[GraphType.None] = null;
        }


        public IGraphable GetConverter(GraphType graphType)
        {
            return _converters[graphType];
        }
    }
}
