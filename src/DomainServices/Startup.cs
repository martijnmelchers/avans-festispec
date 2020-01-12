using Festispec.DomainServices.Factories;
using Festispec.DomainServices.Interfaces;
using Festispec.DomainServices.Services;
using Festispec.Models.EntityMapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using Festispec.DomainServices.Services.Offline;

namespace Festispec.DomainServices
{
    [ExcludeFromCodeCoverage]
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddTransient<FestispecContext>();
            services.AddScoped(typeof(ISyncService<>), typeof(JsonSyncService<>));
            services.AddSingleton<IOfflineService, DbPollOfflineService>();
            string environment = Environment.GetEnvironmentVariable("Environment") ?? "Debug";

            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environment}.json")
                .Build();

            services.AddSingleton<IConfiguration>(config => configuration);

            // Register services for *both* online and offline here
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
                services.AddScoped<ISicknessService, SicknessService>();
                services.AddScoped<IAvailabilityService, AvailabilityService>();

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
            services.AddSingleton(new AnswerFactory());

            return services; 
        }
    }
}
