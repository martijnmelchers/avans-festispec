using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Festispec.UI
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("nl-NL");
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("nl-NL");

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }
    }
}