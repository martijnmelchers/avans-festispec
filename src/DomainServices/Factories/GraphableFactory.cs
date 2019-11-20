using Festispec.Models;
using Festispec.Models.GraphConverters;
using Festispec.Models.Interfaces;
using Festispec.Models.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Festispec.DomainServices.Factories
{
    public class GraphableFactory
    {
        private readonly Dictionary<GraphType, IGraphable> _converters;

        public GraphableFactory()
        {
            _converters = new Dictionary<GraphType, IGraphable>
            {
                [GraphType.Pie] = new PieChartGraphable(),
                [GraphType.Line] = new LineGraphable(),
                [GraphType.Column] = new ColumnGraphable()
            };
        }


        public IGraphable GetConverter(GraphType graphType)
        {
            return _converters[graphType];
        }
    }
}
