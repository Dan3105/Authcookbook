using AuthCookbook.Core.Shared.Models;
using AuthCookbook.Core.Shared.Plugins.HashPassword;
using AuthCookbook.Core.Shared.Repository;

namespace AuthCookbook.Core.Authentication.Login.Session.Service
{
    /// <summary>
    /// Auth Service return sessionId for user after login
    /// </summary>
    public class AuthSessionService : ILoginService<AuthSession>, IAuthSessionService
    {
        private readonly IHashPasswordService hashPasswordService;
        private readonly IRepositoryManager repositoryManager;

        public AuthSessionService(IHashPasswordService hashPasswordService,
            IRepositoryManager repositoryManager)
        {
            this.hashPasswordService = hashPasswordService;
            this.repositoryManager = repositoryManager;
        }

        public AuthSession LoginWithPassword(string usernameOrEmail, string password)
        {
            var userRepo = repositoryManager.GetRepository<UserIdentity>();
            var user = userRepo.Get().FirstOrDefault(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
            if (user == null || !hashPasswordService.VerifyPassword(password, user.HashPassword))
            {
                throw new Exception("Invalid username/email or password.");
            }
            var session = CreateSession(user.Id);
            return session;
        }

        public AuthSession CreateSession(Guid userId)
        {
            var session = new AuthSession
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            repositoryManager.GetRepository<AuthSession>().Add(session);
            repositoryManager.SaveChanges();
            return session;
        }

        public bool IsSessionValid(Guid sessionId)
        {
            var sessionRepo = repositoryManager.GetRepository<AuthSession>();
            var session = sessionRepo.Get().FirstOrDefault(s => s.SessionId == sessionId);
            if (session == null || session.ExpiresAt < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        public void TerminateSession(Guid sessionId)
        {
            var sessionRepo = repositoryManager.GetRepository<AuthSession>();
            var session = sessionRepo.Get().FirstOrDefault(s => s.SessionId == sessionId);
            if (session != null)
            {
                session.ExpiresAt = DateTime.UtcNow;
                sessionRepo.Update(session);
                repositoryManager.SaveChanges();
            }
        }

        public AuthSession LoginWithThirdParty(object signatureData)
        {
            throw new NotImplementedException();
        }
    }
}
