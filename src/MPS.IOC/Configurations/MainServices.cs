using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MPS.Data.EF.Context;
using MPS.Data.EF.Interfaces.UOW;
using MPS.Data.EF.Services.UOW;
using MPS.Domain.Entities.Security;
using MPS.IOC.Configurations.Identity;
using MPS.Services.Interfaces;
using MPS.Services.Interfaces.FileManager;
using MPS.Services.Interfaces.RoleManager;
using MPS.Services.Services;
using MPS.Services.Services.RoleManager;
using System;

namespace MPS.IOC.Configurations
{
    public static class MainServices
    {
        public static void AddMainServices(this IServiceCollection services)
        {
            services.AddScoped<MPSDbContext>();
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
