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
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreCMS.Framework.Core.Auth.Handlers;
using NetCoreCMS.Framework.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.HelloWorld.Handlers
{
    public class HrAuthHandler : INccAuthorizationHandler
    {
        private readonly UserManager<NccUser> _userManager;

        public HrAuthHandler(UserManager<NccUser> userManager)
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
            else if (requirementName.Equals("Organizations"))
            {
                list.Add(requirementName, "OnnoRokomSoftware");
                list.Add(requirementName, "OnnoRokomGroup");
                list.Add(requirementName, "Orelco");
                list.Add(requirementName, "Orelco");
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

            return AuthorizationResult.Success();
        }
    }
}
