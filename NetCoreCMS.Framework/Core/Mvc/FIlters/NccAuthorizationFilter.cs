using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Auth;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccAuthorizationFilter : IAuthorizationFilter
    {
        private readonly NccUserService _nccUserService;

        public NccAuthorizationFilter(NccUserService nccUserService)
        {
            _nccUserService = nccUserService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.IsInRole(NccCmsRoles.SuperAdmin))
            {
                return;
            }

            //var ad = context.ActionDescriptor;
            //var user = context.HttpContext.User;
            //var nccUser = _nccUserService.Get(user.GetUserId());

            //if(nccUser == null)
            //{
            //    context.Result = new ChallengeResult();
            //}
            //else
            //{

            //}
        }
    }
}
