using AuthServer.Infracstructure;
using AuthServer.Infracstructure.External;
using AuthServer.Model.SessionModel;
using AuthServer.Model.UserModel;
using AuthServer.Services.Shared;
using AuthServer.Services.Shared.UnitOfWork;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AuthServer.Services.SessionLogin
{
    public class SessionLoginService(
        IUnitOfWork<InMemoryDbContext> uow,
        IHttpContextAccessor httpContextAccessor) : ISessionLoginService
    {
        public async Task<ResultDTO> LoginByUserName(string userNameOrEmail, string password)
        {
            var user = uow.GetDbContext.Set<User>()
                .FirstOrDefault(u => u.username == userNameOrEmail || u.email == userNameOrEmail);

            if (user == null || string.IsNullOrEmpty(user.hashPassword))
            {
                return ResultDTO.Failure();
            }

            bool isPasswordValid = HashPasswordUtility.VerifyPassword(password, user.hashPassword);
            if (!isPasswordValid)
            {
                return ResultDTO.Failure("Login Failed", 401);
            }

            await SignInUser(user);
            return ResultDTO.Success();
        }
        public async Task<ResultDTO> LogoutCurrentSession()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var user = httpContext?.User;
            if (httpContext == null || user == null)
            {
                return ResultDTO.Failure();
            }

            try
            {
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return ResultDTO.Success();
            } catch (Exception)
            {
                // Log exception if needed
                return ResultDTO.Failure();
            }
        }

        private async Task SignInUser(User user)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                if (httpContext.User != null)
                {
                    await httpContext.SignOutAsync();
                }

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new System.Security.Claims.ClaimsPrincipal(
                        new System.Security.Claims.ClaimsIdentity(
                        [
                            new System.Security.Claims.Claim("UserId", user.id.ToString())
                        ], CookieAuthenticationDefaults.AuthenticationScheme))
                    );
            }
        }
    }
}
