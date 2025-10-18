namespace AuthCookbook.Core.Shared.Plugins.HashPassword
{
    public interface IHashPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyPassword(string providedPassword, string hashPassword);
    }
}
