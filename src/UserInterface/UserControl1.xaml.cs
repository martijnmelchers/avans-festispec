using System;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace Festispec.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(string name)
        {
            InitializeComponent();


            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }
    }
}
