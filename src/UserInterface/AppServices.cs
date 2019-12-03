using Festispec.DomainServices;
using Microsoft.Extensions.DependencyInjection;
using Festispec.UI.ViewModels;
using Festispec.UI.Services;
using Festispec.UI.Interfaces;
using System;

namespace Festispec.UI
{
    public class AppServices
    {
        private AppServices()
        {
            var services = new ServiceCollection();

            //  Register Viewmodels here
            services.AddTransient<MainViewModel>();
            services.AddTransient<FirstTimeViewModel>();
            services.AddTransient<RapportPreviewViewModel>();

            // Services from UI project
            services.AddSingleton<IFrameNavigationService>(RegisterRoutes());

            // Services from DomainServices
            services.AddDomainServices();

            ServiceProvider = services.BuildServiceProvider();
        }

        private static FrameNavigationService RegisterRoutes()
        {
            var navigationService = new FrameNavigationService();

            // Register your routes here
            navigationService.Configure("Homepage", new Uri("../Views/MainWindow.xaml", UriKind.Relative));
            navigationService.Configure("FirstTime", new Uri("../Views/FirstTimePage.xaml", UriKind.Relative));
            navigationService.Configure("RapportPreview", new Uri("../Views/RapportPreviewWindow.xaml", UriKind.Relative));

            return navigationService;
        }

        public IServiceProvider ServiceProvider { get; }

        private static AppServices _instance;
        private static readonly object _instanceLock = new object();
        private static AppServices GetInstance()
        {
            lock (_instanceLock)
            {
                return _instance ?? (_instance = new AppServices());
            }
        }

        public static AppServices Instance => _instance ?? GetInstance();
    }
}
