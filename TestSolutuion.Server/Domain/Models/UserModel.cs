using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Domain.Models
{

    public class UserModel
    {
        public string id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = DefaultStaticData.UserRole;

        public static implicit operator AppUser(UserModel user)
        {
            return new AppUser()
            {
                Id = user.id,
                UserName = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
            };
        }
    }
}
