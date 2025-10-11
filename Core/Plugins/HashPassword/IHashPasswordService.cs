namespace AuthCookbook.Core.Plugins.HashPassword
{
    public interface IHashPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyHashPassword(string providedPassword, string hashPassword);
    }
}
