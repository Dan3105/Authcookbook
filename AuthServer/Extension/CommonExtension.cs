using AuthServer.Infracstructure;
using AuthServer.Model.UserModel;
using AuthServer.Services.Monitoring;
using AuthServer.Services.Registration;
using AuthServer.Services.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AuthServer.Extension
{
    public static class CommonExtension
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services,
           IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                services.AddDbContext<InMemoryDbContext>(o =>
                {
                    o.UseInMemoryDatabase("AuthServiceDb")
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                    });
            }
            // Services
            services.AddScoped<IUnitOfWork<InMemoryDbContext>, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IMonitoringService, MonitoringService>();
            return services;
        }
    }
}
