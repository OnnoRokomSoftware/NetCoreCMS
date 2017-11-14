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
using NetCoreCMS.Framework.Core.Messages;
using System.Linq;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using NetCoreCMS.Framework.Core.Auth;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccAuthFilter : INccActionFilter
    {
        private readonly ILogger _logger;
        private readonly NccUserService _nccUserService;

        public int Order { get { return 100; } }

        public NccAuthFilter(NccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
            _nccUserService = nccUserService;
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
             
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var user = context.HttpContext.User;
            if (user.IsInRole(NccCmsRoles.SuperAdmin))
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

            // Allow actions or controller whoich have AllowAnonymous attribute.
            
            if (actionAttributes.Where(x => x is AllowAnonymousAttribute).Count() > 0)
            {
                return;
            }

            if (controllerAttributes.Where(x=> x is AllowAnonymousAttribute).Count() > 0)
            {
                if(actionAttributes.Where(x => x is NccAuthorize).Count() == 0)
                {
                    return;
                }                
            }

            // Check menu permission.
            
            var nccUser = _nccUserService.Get(user.GetUserId());

            if (nccUser == null)
            {
                context.Result = new ChallengeResult();
                isAuthorized = false;
            }
            else
            {
                if (nccUser.ExtraDenies.Where(
                    x => x.Permission.PermissionDetails.Where(
                        y => y.Action == action.DisplayName
                        && y.Controller == ctrl.ControllerContext.ActionDescriptor.ControllerName
                        ).Count() > 0
                    ).Count() > 0
                )
                {
                    isAuthorized = false;
                }
                else
                {
                    isAuthorized = true;
                }

                if (nccUser.Permissions.Where(
                    x=>x.Permission.PermissionDetails.Where(
                        y=>y.Action == action.DisplayName 
                        && y.Controller == ctrl.ControllerContext.ActionDescriptor.ControllerName
                        ).Count() > 0
                    ).Count() > 0
                )
                {
                    isAuthorized = true;
                }
                else
                {
                    isAuthorized = false;
                }

                if (nccUser.ExtraPermissions.Where(
                    x => x.Permission.PermissionDetails.Where(
                        y => y.Action == action.DisplayName
                        && y.Controller == ctrl.ControllerContext.ActionDescriptor.ControllerName
                        ).Count() > 0
                    ).Count() > 0
                )
                {
                    isAuthorized = true;
                }
                else
                {
                    isAuthorized = false;
                }
            }

            // Check Authorization attributes.
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
                        if(handlerType == null)
                        {
                            GlobalMessageRegistry.RegisterMessage(new GlobalMessage() {
                                For = GlobalMessage.MessageFor.Both,
                                ForUsers = new List<string>() { context.HttpContext.User.Identity.Name },
                                Registrater = "NccAuthFilter",
                                Text = $"No implimentation of handler class name {handlerName} found. Please check handler class name at NccAuthorize attribute.",
                                Type = GlobalMessage.MessageType.Error
                            }, new TimeSpan(0, 0, 30));
                        }
                        else
                        {
                            authorizationService = (INccAuthorizationHandler)context.HttpContext.RequestServices.GetService(handlerType);
                        }
                    }
                    
                    var result = authorizationService.HandleRequirement(context, nccAuthRequirement, model).Result;
                    isAuthorized = result.Succeeded;
                    if (isAuthorized == false)
                    {
                        context.HttpContext.Items["Message"] = "You have not enough permission.";
                        context.HttpContext.Response.Redirect("/Home/NotAuthorized");
                    }
                }
            }

            if (isAuthorized == false)
            {
                context.Result = new ChallengeResult(new AuthenticationProperties());
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
