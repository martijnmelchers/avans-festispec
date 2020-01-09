using System.Collections.Generic;
using Festispec.Models.Questions;

namespace Festispec.Models.Interfaces
{
    public interface IGraphable
    {
        public List<GraphableSeries> TypeToChart(Question question);
    }
}