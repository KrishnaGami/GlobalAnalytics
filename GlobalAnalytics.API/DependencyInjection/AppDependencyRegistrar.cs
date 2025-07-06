using GlobalAnalytics.API.Mapping;
using GlobalAnalytics.API.Services.Auth;
using GlobalAnalytics.Data.Factories;
using GlobalAnalytics.Data.Repositories;
using GlobalAnalytics.Data.Services;
using GlobalAnalytics.Lib.Interfaces;

namespace GlobalAnalytics.API.DependencyInjection
{
    public static class AppDependencyRegistrar
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositories
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<JwtTokenService>();

            // Factory
            services.AddScoped<IExporterFactory, ExporterFactory>();

            // AutoMapper
            services.AddAutoMapper(typeof(ClientMappingProfile));
        }
    }

}
