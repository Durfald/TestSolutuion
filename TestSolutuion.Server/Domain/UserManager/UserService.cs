using Microsoft.AspNetCore.Identity;
using TestSolutuion.Server.Domain.Models;
using TestSolutuion.Server.Database.Models;
using TestSolutuion.Server.Database.Repository.UnitOfWork;

namespace TestSolutuion.Server.Domain.UserManager
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUnitOfWork _repositoryUnitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public UserService(IRepositoryUnitOfWork repositoryUnitOfWork, UserManager<AppUser> userManager)
        {
            _repositoryUnitOfWork = repositoryUnitOfWork;
            _userManager = userManager;
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {

            var existingUser = await _repositoryUnitOfWork.User.FindAsync(x => x.UserName == user.Username);
            if (existingUser.Any())
                throw new ArgumentException("User already exists");

            AppUser appUser = user;
            appUser.Id=Guid.NewGuid().ToString();
            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

            if (user.Role != DefaultStaticData.ManagerRole)
            {
                Random random = new Random();
                int num1 = random.Next(0, 10);
                int num2 = random.Next(0, 10);
                int num3 = random.Next(0, 10);
                int num4 = random.Next(0, 10);
                await _repositoryUnitOfWork.Customer.AddAsync(new Customer
                {
                    Id = appUser.Id,
                    Name = appUser.UserName,
                    Code = $"{num1}{num2}{num3}{num4}-{DateTime.UtcNow.Year}"
                });
            }

            return appUser;
        }
      
        public async Task UpdateUserAsync(string id, UserModel user)
        {
            var customer = await _repositoryUnitOfWork.Customer.GetByIdAsync(id);
            var appUser = await _repositoryUnitOfWork.User.GetByIdAsync(id);
            if (appUser == null)
                throw new ArgumentException("User not found");

            if (!string.IsNullOrEmpty(user.Username))
            {
                if(customer != null)
                    customer.Name = user.Username;
                appUser.UserName = user.Username;
            }

            if (!string.IsNullOrEmpty(user.Email))
                appUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                appUser.PhoneNumber = user.PhoneNumber;
            if(!string.IsNullOrEmpty(user.Role))
                appUser.Role = user.Role;
            if (customer != null)
                await _repositoryUnitOfWork.Customer.UpdateAsync(customer);
            await _repositoryUnitOfWork.User.UpdateAsync(appUser);
        }

        public async Task DeleteUserAsync(string id)
        {
            var delcustomer = await _repositoryUnitOfWork.Customer.DeleteAsync(id);
            var deluser = await _repositoryUnitOfWork.User.DeleteAsync(id);
            if(!deluser || !delcustomer)
                throw new ArgumentException("User not found");
        }

        public async Task<UserModel> GetUserByUsernameAsync(string Username)
        {
            var user = await _repositoryUnitOfWork.User.GetUserByUsernameAsync(Username);
            if(user == null)
                throw new ArgumentException("User not found");

            return user;
        }

        public async Task<UserModel> GetUserByIdAsync(string id)
        {
            var user = await _repositoryUnitOfWork.User.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("User not found");
            return user;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            var users = await _repositoryUnitOfWork.User.GetAllAsync();
            if (users.Count() == 0)
                return [];
            return users.Select(appUser => (UserModel)appUser).ToList();

        }
    }

}
