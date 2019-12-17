using Festispec.DomainServices;
using Microsoft.Extensions.DependencyInjection;
using Festispec.UI.ViewModels;
using Festispec.UI.Services;
using Festispec.UI.Interfaces;
using System;
using Festispec.UI.ViewModels.Employees;

namespace Festispec.UI
{
    public class AppServices
    {
        private AppServices()
        {
            var services = new ServiceCollection();

            //  Register Viewmodels here
            services.AddTransient<MainViewModel>();

            #region Festival ViewModels
            services.AddTransient<FestivalViewModel>();
            services.AddTransient<CreateFestivalViewModel>();
            services.AddTransient<UpdateFestivalViewModel>();
            services.AddTransient<FestivalListViewModel>();
            #endregion

            #region Questionnaire ViewModels
            services.AddTransient<QuestionnaireViewModel>();
            #endregion

            #region Customer ViewModels
            services.AddTransient<CustomerViewModel>();
            services.AddTransient<CustomerListViewModel>();
            #endregion
            
            #region Employee ViewModels
            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<EmployeeListViewModel>();
            #endregion

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

            #region Festival Routes
            navigationService.Configure("FestivalInfo", new Uri("../Views/Festival/FestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("CreateFestival", new Uri("../Views/Festival/CreateFestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateFestival", new Uri("../Views/Festival/UpdateFestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("FestivalList", new Uri("../Views/Festival/FestivalListPage.xaml", UriKind.Relative));
            #endregion

            #region Questionnaire Routes
            navigationService.Configure("Questionnaire", new Uri("../Views/Questionnaire/QuestionnairePage.xaml", UriKind.Relative));
            #endregion

            #region Customer Routes
            navigationService.Configure("CustomerList", new Uri("../Views/Customer/CustomerScreen.xaml", UriKind.Relative));
            navigationService.Configure("NewCustomer", new Uri("../Views/Customer/NewCustomerPage.xaml", UriKind.Relative));
            navigationService.Configure("EditCustomer", new Uri("../Views/Customer/EditCustomerPage.xaml", UriKind.Relative));
            #endregion
            
            #region Employee Routes
            navigationService.Configure("EmployeeInfo", new Uri("../Views/Employee/EmployeePage.xaml", UriKind.Relative));
            navigationService.Configure("CreateEmployee", new Uri("../Views/Employee/CreateEmployeePage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateEmployee", new Uri("../Views/Employee/UpdateEmployeePage.xaml", UriKind.Relative));
            navigationService.Configure("EmployeeList", new Uri("../Views/Employee/EmployeeListPage.xaml", UriKind.Relative));
            #endregion
    
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
