using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.UserManager
{
    public interface IUserService
    {
        Task<UserModel> CreateUserAsync(UserModel user);
        // Добавьте другие методы, например, для получения, обновления и удаления пользователей:
        Task<UserModel> GetUserByIdAsync(string id);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task UpdateUserAsync(string id, UserModel user);
        Task DeleteUserAsync(string id);
        Task<UserModel> GetUserByUsernameAsync(string Username);
    }
}
