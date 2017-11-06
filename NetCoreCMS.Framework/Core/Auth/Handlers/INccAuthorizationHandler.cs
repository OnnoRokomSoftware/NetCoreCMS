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
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Auth.Handlers
{
    public interface INccAuthorizationHandler
    {
        Dictionary<string, string> GetRequirementValues(string requirementName);
        Task<AuthorizationResult> HandleRequirement(ActionExecutingContext context, NccAuthRequirement requirement, object model);
    }
}
