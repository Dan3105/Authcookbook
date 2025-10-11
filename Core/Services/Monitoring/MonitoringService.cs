using AuthCookbook.Core.Models;
using AuthCookbook.Core.Shared.Repository;

namespace AuthCookbook.Core.Services.Users
{
    public class MonitoringService(IRepositoryManager repositoryManager) : IMonitoringService
    {
        public List<UserIdentity> AllUsers()
        {
            var users = repositoryManager.GetRepository<UserIdentity>().Get().ToList();
            return users;
        }
    }
}
