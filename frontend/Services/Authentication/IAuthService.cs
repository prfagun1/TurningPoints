using entities;

namespace frontend.Services
{
    public interface IAuthService
    {
        Task<LoginResult?> Login(LoginViewModel loginModel);
        Task Logout();

    }
}
