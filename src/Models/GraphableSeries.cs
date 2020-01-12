using LiveCharts;

namespace Festispec.Models
{
    public class GraphableSeries
    {
        public string Title { get; set; }
        public IChartValues Values { get; set; }
    }
}