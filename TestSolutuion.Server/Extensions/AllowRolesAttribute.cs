using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace TestSolutuion.Server.Extensions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowRolesAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public AllowRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var Role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (Role == null || !_roles.Contains(Role))
                context.Result = new ForbidResult();
        }
    }
}
