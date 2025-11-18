using AuthServer.Infracstructure;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Model.UserModel
{
    public class UserRepository(InMemoryDbContext dbContext) : IUserRepository
    {
        public async Task<User?> IsDuplicateUserNameOrEmail(string username, string email)
        {
            return await dbContext.Set<User>().FirstOrDefaultAsync(u => u.username == username || u.email == email);
        }
    }
}
