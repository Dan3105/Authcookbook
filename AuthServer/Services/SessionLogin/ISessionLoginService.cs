using AuthServer.Model.SessionModel;
using AuthServer.Services.Shared;

namespace AuthServer.Services.SessionLogin
{
    public interface ISessionLoginService
    {
        Task<ResultDTO> LoginByUserName(string userNameOrEmail, string password);
        Task<ResultDTO> LogoutCurrentSession();
    }
}