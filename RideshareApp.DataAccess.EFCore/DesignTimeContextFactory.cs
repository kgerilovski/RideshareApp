using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RideshareApp.DataAccess.EFCore
{
    class DesignTimeContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        // this class used by EF tool for creating migrations via Package Manager Console
        // Add-Migration MigrationName -Project Sevlievo.DataAccess.EFCore -StartupProject Sevlievo.DataAccess.EFCore
        // or in case of console building: dotnet ef migrations add MigrationName --project Sevlievo.DataAccess.EFCore --startup-project Sevlievo.DataAccess.EFCore
        // after migration is added, please add migrationBuilder.Sql(SeedData.Initial()); to the end of seed method for initial data
        public DesignTimeContextFactory() { }


        public DataContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../API"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("localDb");

            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseSqlite(connectionString);

            var httpContext = new HttpContextAccessor();

            return new DataContext(builder.Options, httpContext);
        }
    }
}
