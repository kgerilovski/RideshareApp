using API.Entities;
using Microsoft.AspNetCore.Identity;
using RideshareApp.Entities;
using RideshareApp.IdentityManagementCore;

namespace API.Setup
{
    public static class IdentityConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.AddTransient<IdentityErrorDescriber>();

            services.AddTransient<IRoleStore<Role>, RoleStore<Role>>();
            services.AddTransient<IUserStore<User>, UserStore<User, Role, UserRole>>();
            services.AddTransient<UserManager<User>, ApplicationUserManager>();
            services.AddTransient(typeof(RoleManager<Role>));

            var identityBuilder = new IdentityBuilder(typeof(User), typeof(User), services);
            identityBuilder.AddDefaultTokenProviders();
        }
    }
}
