namespace TestSolutuion.Server.Domain.AuthService
{
    public interface IAuthService
    {
        Task<(string Token, string Role)> LoginAsync(string username, string password);
    }
}
