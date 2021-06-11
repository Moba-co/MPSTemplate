using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MPS.Common.Helpers;
using MPS.Domain.Entities.Security;
using MPS.Services.Interfaces;
using MPS.Services.Services;
using MPS.Services.Services.DbInitializer;

namespace MPS.IOC.Configurations.Identity
{
    public static class IdentityServicesRegistry
    {
        public static void AddCustomIdentityServices(this IServiceCollection services)
        {
            services.AddIdentityOptions();
            services.AddScoped<SignInManager<User>, SignInManager<User>>();
            services.AddScoped<UserManager<User>, UserManager<User>>();
            services.AddScoped<MPSUserManager, MPSUserManager>();
            services.AddScoped<RoleManager<Role>, RoleManager<Role>>();
            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();
            services.AddScoped<PersianIdentityErrorDescriber>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Auth/Login");
                options.AccessDeniedPath = new PathString("/Auth/AccessDenied");
                options.LogoutPath = new PathString("/Auth/LogOut");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });
        }

        public static void UseCustomIdentityServices(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.CallDbInitializer();
        }

        private static void CallDbInitializer(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var identityDbInitialize = scope.ServiceProvider.GetService<IIdentityDbInitializer>();
            identityDbInitialize?.Initialize();
            identityDbInitialize?.SeedData();
        }
    }
}