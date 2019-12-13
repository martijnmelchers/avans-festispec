using Festispec.UI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Festispec.UI.Views
{
    /// <summary>
    /// Interaction logic for FestivalPage.xaml
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
