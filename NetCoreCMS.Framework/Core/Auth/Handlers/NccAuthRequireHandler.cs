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
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreCMS.Framework.Core.Auth.Handlers
{
    /// <summary>
    /// Default NetCoreCMS authorization handler. If any controller use NccAuthorize attribute then this handler will handle that requirement.
    /// </summary>    
    public class NccAuthRequireHandler : INccAuthorizationHandler
    {
        private readonly UserManager<NccUser> _userManager;        
        
        public NccAuthRequireHandler(UserManager<NccUser> userManager )
        {
            _userManager = userManager;                        
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

        public async Task<AuthorizationResult> HandleRequirement(ActionExecutingContext context, NccAuthRequirement requirement, object model)
        {
            
            if (context.HttpContext.User == null)
            {
                return AuthorizationResult.Failed();
            }

            //var usersPolicyList = _nccUserAuthPolicyService.LoadByModulePolicy(PolicyHandler.NccAuthRequireHandler, requirement);

            if (requirement.Requirement == NccAuthRequirementName.IsDataOwner)
            {
                //if (resource.CreateBy == context.User.GetUserId())
                //{
                //    context.Succeed(requirement);
                //}
            }
            else if(context.HttpContext.User.Identity.IsAuthenticated)
            {
                return AuthorizationResult.Success();
            }

            return AuthorizationResult.Failed();
        }
    }    
}
