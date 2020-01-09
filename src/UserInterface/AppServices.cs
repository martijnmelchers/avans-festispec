using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Festispec.DomainServices;
using Festispec.DomainServices.Enums;
using Festispec.DomainServices.Interfaces;
using Festispec.UI.Interfaces;
using Festispec.UI.Services;
using Festispec.UI.ViewModels;
using Festispec.UI.ViewModels.Customers;
using Festispec.UI.ViewModels.Employees;
using Festispec.UI.ViewModels.Festivals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Festispec.UI
{
    public class AppServices
    {
        private static AppServices _instance;
        private static readonly object _instanceLock = new object();

        private AppServices()
        {
            var services = new ServiceCollection();

            //  Register Viewmodels here
            services.AddSingleton<MainViewModel>();

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
            services.AddTransient<InspectionViewModel>();

            #endregion

            #region Employee ViewModels

            services.AddTransient<EmployeeViewModel>();
            services.AddTransient<EmployeeListViewModel>();
            services.AddTransient<AccountViewModel>();
            services.AddTransient<CertificateListViewModel>();
            services.AddTransient<CertificateViewModel>();

            #endregion


            services.AddTransient<RapportPreviewViewModel>();
            services.AddTransient<MapViewModel>();

            // Services from UI project
            services.AddSingleton<IFrameNavigationService>(RegisterRoutes());

            // Services from DomainServices
            services.AddDomainServices();
            
            // Initialise the application directory structure for WPF.
            // Make sure to add your custom paths here.
            FestispecPaths.Setup();

            // Run an initial offline sync in a background thread
            Task.Run(() =>
            {
                List<ServiceDescriptor> serviceDescriptors = services
                    .Where(x => typeof(ISyncable).IsAssignableFrom(x.ServiceType))
                    .ToList();
                
                foreach (ServiceDescriptor service in serviceDescriptors)
                    ((ISyncable) services.BuildServiceProvider().GetRequiredService(service.ServiceType)).Sync();
            });

            ServiceProvider = services.BuildServiceProvider();
        }

        public IServiceProvider ServiceProvider { get; }

        public static AppServices Instance => _instance ?? GetInstance();

        private static FrameNavigationService RegisterRoutes()
        {
            var navigationService = new FrameNavigationService();

            // Register your routes here

            #region Festival Routes

            navigationService.Configure("FestivalInfo",
                new Uri("../Views/Festival/FestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("CreateFestival",
                new Uri("../Views/Festival/CreateFestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateFestival",
                new Uri("../Views/Festival/UpdateFestivalPage.xaml", UriKind.Relative));
            navigationService.Configure("FestivalList",
                new Uri("../Views/Festival/FestivalListPage.xaml", UriKind.Relative));

            #endregion

            #region inspection route

            navigationService.Configure("Inspection",
                new Uri("../Views/Inspection/InspectionPage.xaml", UriKind.Relative));

            #endregion

            #region Questionnaire Routes

            navigationService.Configure("Questionnaire",
                new Uri("../Views/Questionnaire/QuestionnairePage.xaml", UriKind.Relative));

            #endregion

            #region Customer Routes

            navigationService.Configure("CustomerList",
                new Uri("../Views/Customer/CustomerListPage.xaml", UriKind.Relative));
            navigationService.Configure("CreateCustomer",
                new Uri("../Views/Customer/CreateCustomerPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateCustomer",
                new Uri("../Views/Customer/UpdateCustomerPage.xaml", UriKind.Relative));
            navigationService.Configure("CustomerInfo",
                new Uri("../Views/Customer/CustomerPage.xaml", UriKind.Relative));

            #endregion

            #region Employee Routes

            navigationService.Configure("EmployeeInfo",
                new Uri("../Views/Employee/EmployeePage.xaml", UriKind.Relative));
            navigationService.Configure("CreateEmployee",
                new Uri("../Views/Employee/CreateEmployeePage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateEmployee",
                new Uri("../Views/Employee/UpdateEmployeePage.xaml", UriKind.Relative));
            navigationService.Configure("EmployeeList",
                new Uri("../Views/Employee/EmployeeListPage.xaml", UriKind.Relative));

            navigationService.Configure("UpdateAccount",
                new Uri("../Views/Employee/UpdateAccountPage.xaml", UriKind.Relative));

            navigationService.Configure("CertificateList",
                new Uri("../Views/Employee/CertificateListPage.xaml", UriKind.Relative));
            navigationService.Configure("UpdateCertificate",
                new Uri("../Views/Employee/UpdateCertificatePage.xaml", UriKind.Relative));
            navigationService.Configure("CreateCertificate",
                new Uri("../Views/Employee/CreateCertificatePage.xaml", UriKind.Relative));

            #endregion


            #region Login Routes

            navigationService.Configure("LoginPageEmployee",
                new Uri("../Views/Login/LoginPageEmployee.xaml", UriKind.Relative));

            #endregion

            #region Home Routes

            navigationService.Configure("HomePage", new Uri("../Views/Home/HomePage.xaml", UriKind.Relative));

            #endregion


            navigationService.Configure("GenerateReport",
                new Uri("../Views/RapportPreviewPage.xaml", UriKind.Relative));
            navigationService.Configure("MapPage", new Uri("../Views/Map/MapPage.xaml", UriKind.Relative));


            return navigationService;
        }

        private static AppServices GetInstance()
        {
            lock (_instanceLock)
            {
                return _instance ??= new AppServices();
            }
        }
    }
}