using System.Collections.Generic;
using Festispec.Models.Answers;
using Festispec.Models.Questions;

namespace Festispec.Models.Interfaces
{
    public interface IGraphable
    {
        List<GraphableSeries> ToChart<TQuestion>(TQuestion question) where TQuestion : Question;
    }
}
