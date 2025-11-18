using AuthServer.Infracstructure;
using AuthServer.Infracstructure.External;
using AuthServer.Model.SessionModel;
using AuthServer.Model.UserModel;
using AuthServer.Services.Shared;
using AuthServer.Services.Shared.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AuthServer.Services.Monitoring
{
    public class MonitoringService(
        IConnectionMultiplexer redis,
        ILogger<MonitoringService> logger,
        IUnitOfWork<InMemoryDbContext> uow) : IMonitoringService
    {
        public async Task<ResultDTO<List<SessionDTO>>> getAllSessions(int maximumNumber = 10)
        {
            try
            {
                var sessions = new List<SessionDTO>();
                var endpoints = redis.GetEndPoints();
                var server = redis.GetServer(endpoints.First());

                string pattern = $"{SessionConfiguration.SessionPrefix}*";

                IEnumerable<RedisKey> keys = server.Keys(pattern: pattern, pageSize: maximumNumber);
                foreach (var key in keys)
                {
                    var db = redis.GetDatabase();
                    var sessionData = await db.StringGetAsync(key);
                    if (sessionData.HasValue)
                    {
                        var authTicket = TicketSerializer.Default.Deserialize(sessionData!);
                        if (authTicket != null)
                        {
                            sessions.Add(new SessionDTO
                            {
                                SessionId = key.ToString()!,
                                UserId = int.Parse(authTicket.Principal.FindFirst("UserId")!.Value),
                                CreatedAt = authTicket.Properties.IssuedUtc?.UtcDateTime,
                                ExpiresAt = authTicket.Properties.ExpiresUtc?.UtcDateTime
                            });
                        }
                    }
                }

                return ResultDTO<List<SessionDTO>>.Success(sessions);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving sessions from Redis");
                return ResultDTO<List<SessionDTO>>.Failure($"Error retrieving sessions");
            }
        }
        public async Task<ResultDTO<List<UserDTO>>> getAllUsers()
        {
            var users = await uow.GetDbContext.Set<User>()
                .Select(u => new UserDTO
                {
                    id = u.id,
                    username = u.username,
                    displayName = u.displayName,
                    email = u.email
                })
                .ToListAsync();

            return ResultDTO<List<UserDTO>>.Success(users);
        }
    } 
}
