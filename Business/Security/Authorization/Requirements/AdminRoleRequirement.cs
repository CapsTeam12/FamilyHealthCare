using Microsoft.AspNetCore.Authorization;

namespace Business.Security.Authorization.Requirements
{
    public class AdminRoleRequirement : IAuthorizationRequirement
    {
        public AdminRoleRequirement() {}
    }
}
