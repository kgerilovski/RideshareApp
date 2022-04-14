using API.Setup;
using Microsoft.OpenApi.Models;
using RideshareApp.DataAccess.EFCore.Infrastructure.MappingProfiles;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAuth(Configuration);
            IdentityConfig.Configure(services);


            var mapperConfig = AutoMapperConfig.Configure();
            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.ConfigureDependencies(Configuration);

            services.AddControllers(opt =>
            {
                opt.UseCentralRoutePrefix(new RouteAttribute("api"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
            });

            services.ConfigureCors();

            services.AddMvc(opt => opt.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
