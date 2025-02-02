using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Domain.UserManager
{
    public interface IUserService
    {
        Task<UserModel> CreateUserAsync(UserModel user);
        // Добавьте другие методы, например, для получения, обновления и удаления пользователей:
        Task<UserModel> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task UpdateUserAsync(Guid id, UserModel user);
        Task DeleteUserAsync(Guid id);
        Task<UserModel> GetUserByUsernameAsync(string Username);
    }
}
