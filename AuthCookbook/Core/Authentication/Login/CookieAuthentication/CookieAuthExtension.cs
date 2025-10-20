using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthCookbook.Core.Authentication.Login.CookieAuthentication
{
    public static class CookieAuthExtension
    {
        public static IServiceCollection AddCookieAuthServices(this IServiceCollection services,
            IWebHostEnvironment env)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookieAuthBuilder();

            return services;
        }

        public static AuthenticationBuilder AddCookieAuthBuilder(this AuthenticationBuilder builder)
        {
            return builder.AddCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                option.SlidingExpiration = true;
                option.AccessDeniedPath = "/AccessDenied";
            });
        }
    }
}
