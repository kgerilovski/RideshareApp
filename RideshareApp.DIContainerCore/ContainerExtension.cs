using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RideshareApp.DataAccess.EFCore;
using RideshareApp.DataAccess.EFCore.Infrastructure.Contracts;
using RideshareApp.DataAccess.EFCore.Infrastructure.Impl;
using RideshareApp.Services;
using RideshareApp.Services.Infrastructure;
using RideshareApp.Services.Infrastructure.Services;
using System.Reflection;

namespace RideshareApp.DIContainerCore
{
    public static class ContainerExtension
    {
        public static void Initialize(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlite(connectionString));

            services.AddScoped<IDataBaseInitializer, DatabaseInitializer>();

            var servicesTypes = typeof(UserService).Assembly.GetClassTypes("Service");
            var interfaceTypes = typeof(IUserService).Assembly.GetInterfaceTypes("Service");
            AddTransientTypes(services, servicesTypes, interfaceTypes);

            var repositoryTypes = typeof(UserRepository).Assembly.GetClassTypes("Repository");
            var interfaceRepositoryTypes = typeof(IUserRepository).Assembly.GetInterfaceTypes("Repository");
            AddTransientTypes(services, repositoryTypes, interfaceRepositoryTypes);
        }

        private static List<Type> GetClassTypes(this Assembly assembly, string endingName)
        {
            var result = assembly.GetTypes().Where(x => x.IsClass).Where(x => x.Name.EndsWith(endingName)).ToList();
            return result;
        }

        private static List<Type> GetInterfaceTypes(this Assembly assembly, string endingName)
        {
            var result = assembly.GetTypes().Where(x => x.IsInterface).Where(x => x.Name.EndsWith(endingName)).ToList();
            return result;
        }

        private static void AddTransientTypes(IServiceCollection services, List<Type> serviceTypes, List<Type> interfaceTypes)
        {
            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.Name.Substring(1) == serviceType.Name);
                services.AddTransient(interfaceType, serviceType);
            }
        }
    }
}
