using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moba.Data.EF.Context;
using Moba.Data.EF.Interfaces.UOW;
using Moba.Data.EF.Services.UOW;
using Moba.Domain.Entities.Security;
using Moba.IOC.Configurations.Identity;
using Moba.Services.Interfaces;
using Moba.Services.Interfaces.FileManager;
using Moba.Services.Interfaces.RoleManager;
using Moba.Services.Services;
using Moba.Services.Services.RoleManager;
using System;

namespace Moba.IOC.Configurations
{
    public static class MainServices
    {
        public static void AddMainServices(this IServiceCollection services)
        {
            services.AddScoped<MobaDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IRoleManagerHelper, RoleManagerHelper>();
            services.AddScoped<ISessionService, SessionService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddControllersWithViews().AddNToastNotifyToastr();
            services.AddCustomIdentityServices();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddServices();
            services.AddRazorPages();
            services.AddDistributedMemoryCache();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromDays(31);
            });
            services.AddLocalization();
        }
    }
}
