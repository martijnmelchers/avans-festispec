using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Festispec.Models
{
    public class GraphableSeries
    {
        public string Title { get; set; }
        public IChartValues Values { get; set; }
    }   
}
