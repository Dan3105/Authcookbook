namespace AuthCookbook.Core.Plugins.HashPassword
{
    public class BasicHashPasswordService : IHashPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyHashPassword(string providedPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashPassword);
        }
    }
}
