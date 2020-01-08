using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views.Map
{
    /// <summary>
    ///     Interaction logic for MapPage.xaml
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
        }
    }
}