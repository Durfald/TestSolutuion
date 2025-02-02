using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestSolutuion.Server.Domain.Models;

namespace TestSolutuion.Server.Database.Models
{
    public class AppUser : IdentityUser
    {
        [Column("Role")]
        [Required]
        public string Role { get; set; } = string.Empty;

        public static implicit operator UserModel(AppUser appUser)
        {
            return new UserModel
            {
                id = appUser.Id,
                Email = appUser.Email ?? string.Empty,
                Role = appUser.Role,
                PhoneNumber = appUser.PhoneNumber ?? string.Empty,
                Username = appUser.UserName ?? string.Empty
            };
        }
    }
}
