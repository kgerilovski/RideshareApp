using API.Entities;
using API.Identity;
using API.Interfaces;
using RideshareApp.DIContainerCore;

namespace API.Setup
{
    public static class DependenciesConfig
    {
        public static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("localDb");

            services.AddHttpContextAccessor();

            services.AddSingleton<JwtManager>();

            ContainerExtension.Initialize(services, connectionString);

            services.AddTransient<IAuthenticationService, AuthenticationService<User>>();
        }
    }
}
