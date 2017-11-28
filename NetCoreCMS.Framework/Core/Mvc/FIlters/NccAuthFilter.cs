using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using NetCoreCMS.Framework.Core.Auth.Handlers;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using NetCoreCMS.Framework.Core.Mvc.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using NetCoreCMS.Framework.Core.Mvc.Cache;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccAuthFilter : IAuthorizationFilter
    {
        private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly NccUserService _nccUserService;

        public int Order { get { return 100; } }

        public NccAuthFilter(NccUserService nccUserService, IMemoryCache memoryCache, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
            _nccUserService = nccUserService;
            _cache = memoryCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorized = false;            
            var action = (ControllerActionDescriptor)context.ActionDescriptor;
            var actionAttributes = action.MethodInfo.GetCustomAttributes(true);
            var type = action.ControllerTypeInfo;            
            var controllerAttributes = type.GetCustomAttributes(true);

            // Allow actions or controller whoich have AllowAnonymous attribute.
            if (actionAttributes.Where(x => x is AllowAnonymousAttribute).Count() > 0)
            {
                return;
            }

            if (controllerAttributes.Where(x => x is AllowAnonymousAttribute).Count() > 0)
            {
                if (actionAttributes.Where(x => x is NccAuthorize).Count() == 0)
                {
                    return;
                }
            }

            var user = context.HttpContext.User;
            if (user == null)
            {
                context.Result = new ChallengeResult(new AuthenticationProperties());
                context.HttpContext.Items["ErrorMessage"] = "You are not authenticated.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
                return;
            }

            var nccUser = _cache.GetNccUser(user.GetUserId());
            if (nccUser == null)
            {
                nccUser = _nccUserService.Get(user.GetUserId());
                if(nccUser != null)
                {
                    _cache.SetNccUser(nccUser);
                }
            }

            if (nccUser == null)
            {
                context.Result = new ChallengeResult(new AuthenticationProperties());
                context.HttpContext.Items["ErrorMessage"] = "No user found.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
                return;
            }

            if (user.IsInRole(NccCmsRoles.SuperAdmin))
            {
                return;
            }

            //Allow logged users which action has AllowAuthenticated attribute.
            if (actionAttributes.Where(x => x is AllowAuthenticated).Count() > 0)
            {
                return;
            }

            // Check menu permission.

            bool isRedirect = false;
            bool notFound = false;
            
            foreach (var item in actionAttributes)
            {
                if(item is SubActionOf)
                {
                    var subActionOf = (SubActionOf)item;
                    (notFound, isRedirect, isAuthorized) = IsAuthorized(nccUser, subActionOf.Controller, subActionOf.Action);
                    if (isAuthorized)
                        break;
                }
            }

            if(isAuthorized == false)
            {
                (notFound, isRedirect, isAuthorized) = IsAuthorized(nccUser, action.ControllerName, action.ActionName);
            }
            

            if (notFound)
            {
                var url = action.ControllerName + "/" + action.ActionName;
                context.HttpContext.Items["ErrorMessage"] = $"URL '{url}' not found";
                context.HttpContext.Response.Redirect("/Home/ResourceNotFound");
                return;
            }

            if (isRedirect)
            {
                context.Result = new ChallengeResult(new AuthenticationProperties());
                context.HttpContext.Items["ErrorMessage"] = "You have not enough permission.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
                return;
            }

            if (isAuthorized == false)
            {
                context.Result = new ChallengeResult(new AuthenticationProperties());
                context.HttpContext.Items["ErrorMessage"] = "You do not have enought permission.";                
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
            }
        }

        private (bool NotFound, bool Redirect, bool isAuthorized) IsAuthorized(NccUser nccUser, string controllerName, string actionName)
        {
            bool isAuthorized = false;
            string menuName = "";

            (menuName, controllerName, actionName) = GetControllerActionForThisRequest(controllerName, actionName);

            if (string.IsNullOrEmpty(menuName) == true || string.IsNullOrEmpty(controllerName) == true || string.IsNullOrEmpty(actionName) == true)
            {
                return (true, true, false);
            }

            if (nccUser.ExtraDenies.Where(x => x.Action == actionName && x.Controller == controllerName).Count() > 0)
            {
                return (false, true, false);
            }
            else
            {
                if (nccUser.Permissions.Where(x => x.Permission.PermissionDetails.Where(y => y.Action == actionName && y.Controller == controllerName).Count() > 0).Count() > 0)
                {
                    isAuthorized = true;
                }
                else
                {
                    isAuthorized = nccUser.ExtraPermissions.Where(x => x.Action == actionName && x.Controller == controllerName).Count() > 0;
                }
            }

            return (false, false, isAuthorized);
        }

        private (string Menu, string Controller, string Action) GetControllerActionForThisRequest(string controllerName, string actionName)
        {
            var ca = ControllerActionCache.ControllerActions.Where(x => x.SubAction == actionName && x.SubController == controllerName).FirstOrDefault();
            if (ca != null)
            {
                return (ca.MainMenuName, ca.MainController, ca.MainAction);
            }
            return ("", "", "");
        }
        
        private string GetModuleId(NccController nccController)
        {
            var type = nccController.GetType();
            var assemblyName = type.Assembly.GetName();
            var module = GlobalContext.GetModuleByAssemblyName(assemblyName);
            if (module != null)
            {
                return module.ModuleId;
            }
            return null;
        }
    }
}
