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

            var watch = System.Diagnostics.Stopwatch.StartNew();

            _scope = AppServices.Instance.ServiceProvider.CreateScope();
            Unloaded += (sender, e) => _scope.Dispose();

            DataContext = _scope.ServiceProvider.GetRequiredService<FestivalViewModel>();
            watch.Stop();
            MessageBox.Show($"Time for page init: {watch.ElapsedMilliseconds}");
        }

        async void OnLoad(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var x = (FestivalViewModel)DataContext;
            await Task.Run(() => x.Initialize(1));
            watch.Stop();

            MessageBox.Show($"Time for initialization: {watch.ElapsedMilliseconds}");
        }

        private void OpenCreateNewQuestionnairePopUp(object sender, RoutedEventArgs e)
        {
            if (!DeleteWarning.IsOpen)
            {
                DeleteWarning.IsOpen = true;
            }
        }
    }
}
