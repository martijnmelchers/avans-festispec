using Festispec.UI.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Festispec.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceScope _scope;
        public MainWindow()
        {
            InitializeComponent();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<MainViewModel>();
        }
    }
}
