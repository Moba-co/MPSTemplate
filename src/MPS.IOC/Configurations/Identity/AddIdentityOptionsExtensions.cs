using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moba.Common.Helpers;
using Moba.Data.EF.Context;
using Moba.Domain.Entities.Security;

namespace Moba.IOC.Configurations.Identity
{
    public static class AddIdentityOptionsExtensions
    {
        public static IServiceCollection AddIdentityOptions(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(
                    options => {
                        //Configure Password
                        options.User.RequireUniqueEmail = false;
                        options.User.AllowedUserNameCharacters =
                            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                        options.Password.RequireDigit = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequiredUniqueChars = 0;
                        options.Lockout.AllowedForNewUsers = true;
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.SignIn.RequireConfirmedPhoneNumber = true;
                        options.SignIn.RequireConfirmedAccount = false;
                        options.User.RequireUniqueEmail = false;
                    })
                .AddEntityFrameworkStores<MobaDbContext>()
                .AddErrorDescriber<PersianIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
