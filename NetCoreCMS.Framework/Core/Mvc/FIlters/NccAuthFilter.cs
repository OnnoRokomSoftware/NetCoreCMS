/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccAuthFilter : INccActionFilter
    {
        private readonly ILogger _logger;
        private readonly IAuthorizationService _authorizationService;

        public int Order { get { return 100; } }

        public NccAuthFilter(ILoggerFactory loggerFactory, IAuthorizationService authorizationService)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
            _authorizationService = authorizationService;
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
             
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthorized = false;
            var type = context.Controller.GetType();
            var ctrl = (NccController)context.Controller;

            var attribs = type.GetCustomAttributes(true);

            foreach (var item in attribs)
            {
                if (item is AllowAnonymousAttribute)
                {
                    isAuthorized = true; 
                }

                if (item is NccAuthorize)
                {
                    var attrib = (NccAuthorize)item;
                    var nccAuthRequirement = GetNccAuthRequirement(attrib, ctrl);
                    var result = _authorizationService.AuthorizeAsync(context.HttpContext.User, ctrl.ViewData.Model, nccAuthRequirement).Result;
                    isAuthorized = result.Succeeded;
                }
            }

            if(isAuthorized == false)
            {
                context.HttpContext.Items["Message"] = "You have not enough permission.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
            }
        }

        private NccAuthRequirement GetNccAuthRequirement(NccAuthorize attrib, NccController nccController)
        {
            var authRequirement = new NccAuthRequirement(attrib.GetRequirement(), attrib.GetValues());
            authRequirement.RequirementList = attrib.RequirementList;
            authRequirement.ValueList = attrib.ValueList;
            var moduleId = GetModuleId(nccController);
            authRequirement.ModuleId = moduleId;
            return authRequirement;
        }

        private string GetModuleId(NccController nccController)
        {
            var type = nccController.GetType();
            var assemblyName = type.Assembly.GetName();
            var module = GlobalContext.GetModuleByAssemblyName(assemblyName);
            if(module != null)
            {
                return module.ModuleId;
            }
            return null;
        }
    }
}
