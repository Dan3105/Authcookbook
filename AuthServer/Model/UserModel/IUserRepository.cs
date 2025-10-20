namespace AuthServer.Model.UserModel
{
    public interface IUserRepository
    {
        public Task<User?> IsDuplicateUserNameOrEmail(string username, string email);
    }
}