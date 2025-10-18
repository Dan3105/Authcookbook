using AuthCookbook.Core.Authentication.Login.Session.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthCookbook.Core.Authentication.Login.Session
{
    public static class AuthSessionExtension
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services,
            IWebHostEnvironment env)
        {
            services.AddScoped<IAuthSessionService, AuthSessionService>();
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            return services;
        }
    }
}
