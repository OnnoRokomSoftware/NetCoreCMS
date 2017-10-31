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
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using NetCoreCMS.Framework.Core.Mvc.Models;
using NetCoreCMS.Framework.Core.Services;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Auth.Handlers
{   
    /// <summary>
    /// Default NetCoreCMS authorization handler. If any controller use NccAuthorize attribute then this handler will handle that requirement.
    /// </summary>    
    public class NccAuthRequireHandler : AuthorizationHandler<NccAuthRequirement, BaseModel<long>>, INccAuthorizationHandler
    {
        private readonly UserManager<NccUser> _userManager;
        private readonly RoleManager<NccUser> _roleManager;
        private readonly NccUserAuthPolicyService _nccUserAuthPolicyService;

        public NccAuthRequireHandler(UserManager<NccUser> userManager, RoleManager<NccUser> roleManager, NccUserAuthPolicyService nccUserAuthPolicyService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _nccUserAuthPolicyService = nccUserAuthPolicyService;
        }

        protected override Task  HandleRequirementAsync(AuthorizationHandlerContext context, NccAuthRequirement requirement, BaseModel<long> resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.FromResult(0);
            }

            //var usersPolicyList = _nccUserAuthPolicyService.LoadByModulePolicy(PolicyHandler.NccAuthRequireHandler, requirement);

            if(requirement.Name == AuthRequirementName.HasRoles)
            {

            }
            else if(requirement.Name == AuthRequirementName.UserNames)
            {

            }
            else if(requirement.Name == AuthRequirementName.IsDataOwner)
            {
                if (resource.CreateBy == context.User.GetUserId())
                {
                    context.Succeed(requirement);
                }
            }
            else
            {

            }
            
            return Task.FromResult(0);
        } 
    }    
}
