using AuthCookbook.Core.Authentication.Login.Session;

namespace AuthCookbook.ServiceSetup
{
    public static class MainServiceConfiguration
    {
        public static IServiceCollection AddSetupServices(this IServiceCollection services,
           IWebHostEnvironment env)
        {
            services.AddCommonServices(env);
            services.AddAuthServices(env);
            return services;
        }
    }
}
