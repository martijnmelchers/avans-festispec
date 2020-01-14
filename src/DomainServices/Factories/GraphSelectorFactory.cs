using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Festispec.Models;
using Festispec.Models.GraphConverters;
using Festispec.Models.Interfaces;

namespace Festispec.DomainServices.Factories
{
    [ExcludeFromCodeCoverage]
    public class GraphSelectorFactory
    {
        private readonly Dictionary<GraphType, IGraphable> _converters;

        public GraphSelectorFactory()
        {
            _converters = new Dictionary<GraphType, IGraphable>
            {
                [GraphType.Pie] = new ChartGraphable(),
                [GraphType.Line] = new LineGraphable(),
                [GraphType.Column] = new ColumnGraphable(),
                [GraphType.None] = null
            };
        }


        public IGraphable GetConverter(GraphType graphType)
        {
            return _converters[graphType];
        }
    }
}