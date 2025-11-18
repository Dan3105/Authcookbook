using AuthServer.Model.SessionModel;
using AuthServer.Model.UserModel;
using AuthServer.Services.Shared;

namespace AuthServer.Services.Monitoring
{
    public interface IMonitoringService
    {
        Task<ResultDTO<List<UserDTO>>> getAllUsers();
        Task<ResultDTO<List<SessionDTO>>> getAllSessions(int maximumNumber=10);
    } 
}
