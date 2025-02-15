using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.AuthService
{
    public interface IAuthService
    {
        Task<AuthModel> LoginAsync(string username, string password);
    }
}
