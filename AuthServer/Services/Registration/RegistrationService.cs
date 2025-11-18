using AuthServer.Infracstructure;
using AuthServer.Model.SessionModel;
using AuthServer.Model.UserModel;
using AuthServer.Services.Shared;
using AuthServer.Services.Shared.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Services.Registration
{
    public class RegistrationService(
        IUserRepository userRepository, 
        IUnitOfWork<InMemoryDbContext> uow) : IRegistrationService
    {
        public async Task<ResultDTO<User>> RegisterUser(UserRegistrationDTO userRegistrationDTO)
        {
            var existingUser =
                await userRepository.IsDuplicateUserNameOrEmail(userRegistrationDTO.username!, userRegistrationDTO.email!);
            if (existingUser != null)
            {
                return ResultDTO<User>.Failure(message: "Registration Failed - DUP");
            }
            else
            {
                try
                {
                    uow.OpenConnection();
                    var userRegistration = new User
                    {
                        username = userRegistrationDTO.username!,
                        displayName = userRegistrationDTO.username!,
                        email = userRegistrationDTO.email!,
                        hashPassword = BCrypt.Net.BCrypt.HashPassword(userRegistrationDTO.password!)
                    };

                    var createdUser = await uow.GetDbContext.Set<User>().AddAsync(userRegistration);

                    uow.Commit();
                    return ResultDTO<User>.Success(response: createdUser.Entity);
                }
                catch (Exception ex)
                {
                    uow.RollbackConnection();
                    return ResultDTO<User>.Failure(message: $"Registration Failed - EXC: {ex.Message}");
                }
            }
        }
    }
}
