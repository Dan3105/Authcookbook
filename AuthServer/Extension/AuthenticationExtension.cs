using AuthServer.Infracstructure.External;
using AuthServer.Model.SessionModel;
using AuthServer.Services.SessionLogin;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthServer.Extension
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services,
           IConfiguration configuration)
        {
            // Authentication
            services.AddCascadingAuthenticationState();
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.ExpireTimeSpan = SessionConfiguration.SessionDurationInHour;
                    o.SlidingExpiration = true;
                    o.AccessDeniedPath = "/access-denied";
                    o.SessionStore = services.BuildServiceProvider().GetRequiredService<ISessionRedisService>();
                });
           
            services.AddScoped<ISessionLoginService, SessionLoginService>();
            return services;
        }
    }
}
