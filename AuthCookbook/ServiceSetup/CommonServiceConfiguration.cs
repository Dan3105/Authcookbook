using AuthCookbook.Core.Authentication.Registration;
using AuthCookbook.Core.Monitoring;
using AuthCookbook.Core.Shared.Plugins;
using AuthCookbook.Core.Shared.Plugins.HashPassword;
using AuthCookbook.Core.Shared.Repository;
using AuthCookbook.Core.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AuthCookbook.ServiceSetup
{
    public static class CommonServiceConfiguration
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<InMemoryDbContext>(o =>
                {
                    o.UseInMemoryDatabase("AuthCookbookDb");
                });
            }
            services.AddHttpContextAccessor();

            // Services
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRepositoryManager, RepositoryManager>();
            services.AddTransient<IHashPasswordService, BasicHashPasswordService>();
            services.AddTransient<IRegistrationService, BasicRegistrationService>();
            services.AddTransient<IMonitoringService, MonitoringService>();

            return services;
        }
    }
}
