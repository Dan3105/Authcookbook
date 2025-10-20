using AuthCookbook.Core.Shared.Plugins;
using AuthServer.Model.UserModel;
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
            services.AddTransient<IUnitOfWork<InMemoryDbContext>, UnitOfWork>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            return services;
        }
    }
}
