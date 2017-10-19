/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NetCoreCMS.Framework.Core.Auth
{
    public class AuthRequireHandler : AuthorizationHandler<AuthRequire>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequire requirement)
        {
            if (!context.User.Identity.IsAuthenticated || !context.User.HasClaim(c => c.Type == ClaimTypes.Role ) )
            {
                // .NET 4.x -> return Task.FromResult(0);
                return Task.CompletedTask;
            }

            var isSameUser = true;
            var hasRole = true;
            
            if (!string.IsNullOrEmpty(requirement.User))
            {
                isSameUser = context.User.Identity.Name == requirement.User;
            }

            if (!string.IsNullOrEmpty(requirement.Role))
            {
                hasRole = context.User.IsInRole(requirement.Role);
            }

            if (isSameUser && hasRole )
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
