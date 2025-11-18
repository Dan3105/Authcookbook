using AuthServer.Infracstructure;
using AuthServer.Infracstructure.External;
using Docker.DotNet;
using Docker.DotNet.Models;
using StackExchange.Redis;

namespace AuthServer.Extension
{
    public static class ExternalExtension
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Redis Service
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(RedisConstant.RedisConnectionString);
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddScoped<ISessionRedisService, SessionRedisService>();

            // Other external services can be added here
            return services;
        }
    }
}
