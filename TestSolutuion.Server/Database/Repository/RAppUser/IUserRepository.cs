using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.RAppUser
{
    public interface IAppUserRepository : IRepository<AppUser>
    {
        /// <summary>
        /// Получение пользователя по username
        /// </summary>
        /// <param name="username">имя пользователя</param>
        /// <returns></returns>
        public Task<AppUser?> GetUserByUsernameAsync(string username);
    }
}
