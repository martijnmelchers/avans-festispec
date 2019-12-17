using Festispec.UI.ViewModels;
using Festispec.UI.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Festispec.UI.Views.Map
{
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
