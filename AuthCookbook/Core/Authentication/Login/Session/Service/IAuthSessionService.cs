namespace AuthCookbook.Core.Authentication.Login.Session.Service
{
    public interface IAuthSessionService
    {
        public AuthSession CreateSession(Guid userId);
        public bool IsSessionValid(Guid sessionId);
        public void TerminateSession(Guid sessionId);
    }
}
