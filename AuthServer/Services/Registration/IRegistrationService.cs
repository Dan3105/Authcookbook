using AuthServer.Model.UserModel;
using AuthServer.Services.Shared;

namespace AuthServer.Services.Registration
{
    public interface IRegistrationService
    {
        public Task<ResultDTO<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO);
    }
}