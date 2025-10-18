using AuthCookbook.Core.Authentication.Login.Session;
using AuthCookbook.Core.Shared.Models;

namespace AuthCookbook.Core.Monitoring
{
    public interface IMonitoringService
    {
        public List<UserIdentity> AllUsers();
        public List<AuthSession> AllSessions();
    }
}
