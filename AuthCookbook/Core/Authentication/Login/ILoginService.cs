namespace AuthCookbook.Core.Authentication.Login
{
    public interface ILoginService<T> where T : class
    {
        public T LoginWithPassword(string usernameOrEmail, string password);
        public T LoginWithThirdParty(object signatureData);
    }
}
