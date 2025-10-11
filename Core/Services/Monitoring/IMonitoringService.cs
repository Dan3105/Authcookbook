using AuthCookbook.Core.Models;

namespace AuthCookbook.Core.Services.Users
{
    public interface IMonitoringService
    {
        public List<UserIdentity> AllUsers();
    }
}
