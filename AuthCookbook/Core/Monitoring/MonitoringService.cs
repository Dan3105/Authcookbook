using AuthCookbook.Core.Authentication.Login.Session;
using AuthCookbook.Core.Shared.Models;
using AuthCookbook.Core.Shared.Repository;

namespace AuthCookbook.Core.Monitoring
{
    public class MonitoringService(IRepositoryManager repositoryManager) : IMonitoringService
    {
        public List<UserIdentity> AllUsers()
        {
            var users = repositoryManager.GetRepository<UserIdentity>().Get().ToList();
            return users;
        }

        public List<AuthSession> AllSessions()
        {
            var sessions = repositoryManager.GetRepository<AuthSession>().Get().ToList();
            return sessions;
        }
    }
}
