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
using NetCoreCMS.Framework.Core.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Core.Auth.Handlers
{
    /// <summary>
    /// Default NetCoreCMS authorization handler. If any controller use NccAuthorize attribute then this handler will handle that requirement.
    /// </summary>        
    [PolicyHandler("NccAuthRequireHandler")]
    public class NccAuthRequireHandler : AuthorizationHandler<NccAuthRequirement, BaseModel<long>>, INccAuthorizationHandler
    {
        private readonly UserManager<NccUser> _userManager;        
        
        public NccAuthRequireHandler(UserManager<NccUser> userManager )
        {
            _userManager = userManager;                        
        }
        
        protected override Task  HandleRequirementAsync(AuthorizationHandlerContext context, NccAuthRequirement requirement, BaseModel<long> resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.FromResult(0);
            }

            //var usersPolicyList = _nccUserAuthPolicyService.LoadByModulePolicy(PolicyHandler.NccAuthRequireHandler, requirement);

            if(requirement.Requirement == AuthRequirementName.HasRoles)
            {

            }
            else if(requirement.Requirement == AuthRequirementName.UserNames)
            {

            }
            else if(requirement.Requirement == AuthRequirementName.IsDataOwner)
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

        public Dictionary<string, string> GetRequirementValues(string requirementName)
        {
            var list = new Dictionary<string, string>();
            if (requirementName.Equals("Brunches"))
            {
                list.Add(requirementName, "Firmget");
                list.Add(requirementName, "Mirpur1");
                list.Add(requirementName, "Mirpur2");
            }            
            return list;
        }
    }    
}
