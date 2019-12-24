using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MapControl;
namespace Festispec.UI.Views.Map
{
    /// <summary>
    /// Interaction logic for MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        private readonly IServiceScope _scope;

        public MapPage()
        {
            InitializeComponent();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<MapViewModel>();
            BingMapsTileLayer.ApiKey = "Ag2i7B-Uw8sWueLGS7BX7J5xYYKPJnynHsz7KYPQuE_cZAZItqMIQtYgE9mWIvkH";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
