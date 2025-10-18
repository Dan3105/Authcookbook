namespace AuthCookbook.Core.Authentication.Registration
{
    public interface IRegistrationService
    {
        public void RegisterWithPassword(string username, string email, string password);
        public void RegisterWithThirdParty<T>(string username, string email, T signatureData) where T : class;
    }
}
