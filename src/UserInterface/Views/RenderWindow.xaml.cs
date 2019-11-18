using LiveCharts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for RenderWindow.xaml
    /// </summary>
    public partial class RenderWindow : Window
    {
        public RenderWindow()
        {
            InitializeComponent();

            var control1 = new UserControl1("Saus");

            control1.Height = 300;
            control1.Width = 300;
            control1.PointLabel = chartPoint =>
            string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);


            Charts.Children.Add(control1);
        }
    }
}
