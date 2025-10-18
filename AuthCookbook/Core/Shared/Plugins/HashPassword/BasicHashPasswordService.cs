namespace AuthCookbook.Core.Shared.Plugins.HashPassword
{
    public class BasicHashPasswordService : IHashPasswordService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string providedPassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashPassword);
        }
    }
}
