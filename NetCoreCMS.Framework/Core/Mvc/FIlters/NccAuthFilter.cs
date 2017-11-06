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
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Auth.Handlers;
using System.Collections.Generic;
using System;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccAuthFilter : INccActionFilter
    {
        private readonly ILogger _logger;
        
        public int Order { get { return 100; } }

        public NccAuthFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();            
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
             
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (IsNetCoreCMSWeb(context))
            {
                return;
            }

            var isAuthorized = true;
            var type = context.Controller.GetType();
            var ctrl = (NccController)context.Controller;
             
            var action = context.ActionDescriptor;
            var model = ctrl.ViewData.Model;

            var actionAttributes = ctrl.ControllerContext.ActionDescriptor.MethodInfo.GetCustomAttributes(true);
            var controllerAttributes = type.GetCustomAttributes(true);
            
            foreach (var item in actionAttributes)
            {
                if (item is NccAuthorize)
                {
                    var attrib = (NccAuthorize)item;
                    var nccAuthRequirement = GetNccAuthRequirement(attrib, ctrl);
                    var authorizationService = (INccAuthorizationHandler)context.HttpContext.RequestServices.GetService(typeof(NccAuthRequireHandler));
                    var handlerName = attrib.GetHandlerClassName();

                    if (handlerName != nameof(NccAuthRequireHandler))
                    {
                        var handlerType = GlobalContext.GetTypeContainsInModules(handlerName);
                        authorizationService = (INccAuthorizationHandler)context.HttpContext.RequestServices.GetService(handlerType);
                    }
                    
                    var result = authorizationService.HandleRequirement(context, nccAuthRequirement, model).Result;
                    isAuthorized = result.Succeeded;
                }
            }

            if(isAuthorized == false)
            {
                context.HttpContext.Items["Message"] = "You have not enough permission.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
            }
        }

        private bool IsNetCoreCMSWeb(ActionExecutingContext context)
        {
            var type = context.Controller.GetType();
            var assemblyName = type.Assembly.GetName().Name;
            if(assemblyName == "NetCoreCMS.Web")
            {
                return true;
            }
            return false;
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
