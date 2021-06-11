using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MPS.IOC.Configurations.Mapper;
using MPS.Common.ViewModels.Base;
using MPS.Data.EF.Context;
using MPS.IOC.Configurations.ToastNotify;
using MPS.Services.Interfaces.RoleManager;
using MPS.Services.Services.RoleManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using MPS.IOC.Configurations;
using MPS.Services.Services.SignalR;
using MPS.IOC.Configurations.Identity;

namespace MPS.WebApp.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MPSDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddMainServices();

            services.AddMapperConfigurations();

            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.AddHttpClient();

            //services.AddControllersWithViews()

            if (Environment.IsDevelopment())
            {
                services.AddMvcCore().AddToastWithOptions().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddAuthorization()
                    .AddRazorRuntimeCompilation().AddRazorViewEngine();
            }
            else
            {
                services.AddMvcCore().AddToastWithOptions().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                    .AddAuthorization()
                    .AddRazorViewEngine().AddNToastNotifyToastr();
            }

            services.AddAuthorization(option =>
            {
                option.AddPolicy("DynamicRole", policy =>
                    policy.Requirements.Add(new DynamicRoleRequirement()));
            });
            services.AddScoped<IAuthorizationHandler, DynamicRoleManager>();
            services.ConfigureApplicationCookie(options =>
            {
                // You do not have the necessary access 
                options.AccessDeniedPath = "/Auth/AccessDenied";
                options.Cookie.Name = "IdentityProject";
                options.LoginPath = "/Auth/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            });

            services.Configure<SecurityStampValidatorOptions>(option =>
            {
                //Time checking security 
                option.ValidationInterval = TimeSpan.FromSeconds(10);
            });
            //signalr
            services.AddSignalR();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCustomIdentityServices();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<SignalrService>("/SignalrService");
                endpoints.MapControllerRoute(
                     name: "areas",
                     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}