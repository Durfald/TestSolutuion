
using Microsoft.EntityFrameworkCore;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Database.Repository.RAppUser
{
    public class AppUserRepository : Repository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(SQLiteDataBaseContext context) : base(context) { }

        public Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return _context.Users.Where(x=>x.UserName==username).FirstOrDefaultAsync();
        }
    }
}
