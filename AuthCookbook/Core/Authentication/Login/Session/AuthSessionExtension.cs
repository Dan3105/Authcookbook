using AuthCookbook.Core.Authentication.Login.CookieAuthentication;
using AuthCookbook.Core.Authentication.Login.Session.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthCookbook.Core.Authentication.Login.Session
{
    public static class AuthSessionExtension
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services,
            IWebHostEnvironment env)
        {
            // NOTE: ONLY ADD ONE AUTHENTICATION METHOD HERE,
            // THIS PRACTICE IS NOT USING FOR MULTI-AUTHENTICATION
            services.AddCookieAuthServices(env);
            return services;
        }
    }
}
