using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using TestSolutuion.Server.Database.Models;

namespace TestSolutuion.Server.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class DenyRolesAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public DenyRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
            if (userManager == null)
                throw new Exception("UserManager is null");

            var user = await userManager.GetUserAsync(context.HttpContext.User);
            if (user == null || _roles.Contains(user.Role))
                context.Result = new ForbidResult();
        }
    }
}
