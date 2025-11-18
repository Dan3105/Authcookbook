using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthServer.Infracstructure.External
{
    public interface ISessionRedisService : ITicketStore
    {
    }
}