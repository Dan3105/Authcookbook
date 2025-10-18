using AuthCookbook.Core.Shared.Models;
using AuthCookbook.Core.Shared.Plugins.HashPassword;
using AuthCookbook.Core.Shared.Repository;

namespace AuthCookbook.Core.Authentication.Registration
{
    public class BasicRegistrationService(IHashPasswordService hashPasswordService, IRepositoryManager repositoryManager) : IRegistrationService
    {
        private readonly IHashPasswordService _hashPasswordService = hashPasswordService;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        public void RegisterWithPassword(string username, string email, string password)
        {
            if (IsUserExist(username, email))
            {
                throw new Exception("User with the same username or email already exists.");
            }

            var hashPassword = _hashPasswordService.HashPassword(password);
            var userRepo = _repositoryManager.GetRepository<UserIdentity>();
            var newUser = new UserIdentity
            {
                Username = username,
                Email = email,
                HashPassword = hashPassword
            };
            userRepo.Add(newUser);
            _repositoryManager.SaveChanges();
        }

        public void RegisterWithThirdParty<T>(string username, string email, T signatureData) where T : class
        {
            throw new NotImplementedException();
        }

        private bool IsUserExist(string username, string email)
        {
            var userRepo = _repositoryManager.GetRepository<UserIdentity>();
            var existingUser = userRepo.Get().FirstOrDefault(u => u.Username == username || u.Email == email);
            return existingUser != null;
        }
    }
}
