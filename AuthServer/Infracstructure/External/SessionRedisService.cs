
using AuthServer.Model.SessionModel;
using Microsoft.AspNetCore.Authentication;
using StackExchange.Redis;

namespace AuthServer.Infracstructure.External
{
    public class SessionRedisService(IConnectionMultiplexer redis, ILogger<SessionRedisService> logger) : ISessionRedisService
    {
        private readonly IDatabase db = redis.GetDatabase();

        public async Task RemoveAsync(string key)
        {
            await db.KeyDeleteAsync(key);
        }

        public async Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var saveSuccess = await db.StringSetAsync(
                key,
                TicketSerializer.Default.Serialize(ticket),
                SessionConfiguration.SessionDurationInHour,
                when: When.Exists
            );

            if (!saveSuccess)
            {
                throw new KeyNotFoundException("Something wrong with Server");
            }
        }

        public async Task<AuthenticationTicket?> RetrieveAsync(string key)
        {
            var value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                logger.LogDebug("Session key {Key} not found in Redis.", key);
                return null;
            }

            var ticket = TicketSerializer.Default.Deserialize(value!);
            return ticket;
        }

        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var keyTicketPair = GenerateKey(ticket);
            await db.StringSetAsync(
                keyTicketPair.Key,
                keyTicketPair.Value,
                SessionConfiguration.SessionDurationInHour
            );
            return keyTicketPair.Key;
        }

        private KeyValuePair<string, byte[]?> GenerateKey(AuthenticationTicket ticket)
        {
            var key = $"{SessionConfiguration.SessionPrefix}{Guid.NewGuid()}";
            var serializeTicket = TicketSerializer.Default.Serialize(ticket);

            return new KeyValuePair<string, byte[]?>(key, serializeTicket);
        }
    }
}
