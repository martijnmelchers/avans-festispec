using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    /// <summary>
    ///     Interaction logic for UpdateFestivalPage.xaml
    /// </summary>
    public partial class UpdateFestivalPage : Page
    {
        private readonly IServiceScope _scope;

        public UpdateFestivalPage()
        {
            InitializeComponent();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<UpdateFestivalViewModel>();
        }
    }
}