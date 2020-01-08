using System.Windows;
using System.Windows.Controls;
using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI.Views
{
    /// <summary>
    ///     Interaction logic for FestivalPage.xaml
    /// </summary>
    public partial class FestivalPage : Page
    {
        private readonly IServiceScope _scope;

        public FestivalPage()
        {
            InitializeComponent();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<FestivalViewModel>();
        }

        private void ToggleNewQuestionnairePopUp(object sender, RoutedEventArgs e)
        {
            CreateQuestionnairePopUp.IsOpen = !CreateQuestionnairePopUp.IsOpen;
        }

        private void ToggleDeleteQuestionnairePopUp(object sender, RoutedEventArgs e)
        {
            DeleteQuestionnairePopUp.IsOpen = !DeleteQuestionnairePopUp.IsOpen;
        }
    }
}