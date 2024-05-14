using Microsoft.Extensions.DependencyInjection;
using System;

namespace Attar.CRUD.PL.Extentions
{
    public static class AddCookiesServices
    {
        public static IServiceCollection AddApplicationCookiesServices(this IServiceCollection services)
        {

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/Home/Error";

            });

            return services;
        }
    }
}
