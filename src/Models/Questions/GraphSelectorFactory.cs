using System.Collections.Generic;
using Festispec.Models.GraphConverters;
using Festispec.Models.Interfaces;

namespace Festispec.Models.Questions
{
    public class GraphSelectorFactory
    {
        private readonly Dictionary<GraphType, IGraphable> _converters;

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