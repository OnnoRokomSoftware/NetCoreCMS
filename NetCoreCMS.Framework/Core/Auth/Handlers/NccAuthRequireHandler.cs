/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Auth.Handlers
{   
    /// <summary>
    /// Default NetCoreCMS authorization handler. If any controller use NccAuthorize attribute then this handler will handle that requirement.
    /// </summary>
    [NccAuthPolicy("NccAuthRequireHandler", "Default NetCoreCMS Authorization Requirement")]
    public class NccAuthRequireHandler : AuthorizationHandler<NccAuthRequirement, BaseModel<long>>, INccAuthorizationHandler
    {
        private readonly UserManager<NccUser> _userManager;
        private readonly RoleManager<NccUser> _roleManager;

        public NccAuthRequireHandler(UserManager<NccUser> userManager, RoleManager<NccUser> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        protected override Task  HandleRequirementAsync(AuthorizationHandlerContext context, NccAuthRequirement requirement, BaseModel<long> resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.FromResult(0);
            }

            if (resource.CreateBy == context.User.GetUserId())
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }

    [NccAuthPolicy("NccAuthRequireWithValueHandler", "Default NetCoreCMS Authorization Requirement")]
    public class NccAuthRequireWithValueHandler : AuthorizationHandler<NccAuthRequirementWithValue>, INccAuthorizationHandler
    {
        UserManager<NccUser> _userManager;

        public NccAuthRequireWithValueHandler(UserManager<NccUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NccAuthRequirementWithValue requirement)
        {
            if (context.User == null)
            {
                return Task.FromResult(0);
            }

            if(requirement.Value == "Check with users permission value")
            {
                context.Succeed(requirement);
            }

            return Task.FromResult(0);
        }
    }
}
