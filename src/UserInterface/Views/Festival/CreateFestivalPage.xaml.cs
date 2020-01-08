using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    /// <summary>
    ///     Interaction logic for CreateFestivalPage.xaml
    /// </summary>
    public partial class CreateFestivalPage : Page
    {
        private readonly IServiceScope _scope;

        public CreateFestivalPage()
        {
            InitializeComponent();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<CreateFestivalViewModel>();
        }
    }
}