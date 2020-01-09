using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models.EntityMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Festispec.DomainServices
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<FestispecContext>();
            services.AddScoped(typeof(ISyncService<>), typeof(JsonSyncService<>));
            services.AddSingleton<IOfflineService, DbPollOfflineService>();
            string environment = Environment.GetEnvironmentVariable("Environment") ?? "Debug";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            services.AddSingleton<IConfiguration>(config => configuration);

            // Register services for *both* online and offline here
            services.AddScoped<IExampleService, ExampleService>();
            
            // Register all your online services here
            if (services.BuildServiceProvider().GetRequiredService<IOfflineService>().IsOnline)
            {
                services.AddScoped<IAuthenticationService, AuthenticationService>();
                services.AddScoped<IQuestionnaireService, QuestionnaireService>();
                services.AddScoped<IFestivalService, FestivalService>();
                services.AddScoped<ICustomerService, CustomerService>();
                services.AddScoped<IEmployeeService, EmployeeService>();
                services.AddScoped<IInspectionService, InspectionService>();
                services.AddScoped<IAddressService, AddressService>();
                services.AddScoped<IGoogleMapsService, GoogleMapsService>();
                
                // Database initialisation code below
                using (var ctx = services.BuildServiceProvider().GetRequiredService<FestispecContext>()) ctx.Database.Initialize(false);
            }
            else
            {
                services.AddScoped<IAuthenticationService, OfflineAuthenticationService>();
                services.AddScoped<IQuestionnaireService, OfflineQuestionnaireService>();
                services.AddScoped<IFestivalService, OfflineFestivalService>();
                services.AddScoped<ICustomerService, OfflineCustomerService>();
                services.AddScoped<IEmployeeService, OfflineEmployeeService>();
                services.AddScoped<IInspectionService, OfflineInspectionService>();
                services.AddScoped<IAddressService, OfflineAddressService>();
                services.AddScoped<IGoogleMapsService, OfflineGoogleMapsService>();
            }

            // Register all your factories here
            // Example: services.AddSingleton(new ExampleFactory());
            services.AddSingleton(new QuestionFactory());
            services.AddSingleton(new GraphSelectorFactory());

            return services; 
        }
    }
}