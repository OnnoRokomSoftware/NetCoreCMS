/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Mvc.Extensions;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using NetCoreCMS.Framework.Core.Mvc.Attributes;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using NetCoreCMS.Framework.Core.Mvc.Cache;
using System.Collections.Generic;
using NetCoreCMS.Framework.Utility;
using Microsoft.AspNetCore.Identity;

namespace NetCoreCMS.Framework.Core.Mvc.Filters
{
    public class NccAuthFilter : IAuthorizationFilter
    {
        //private IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly INccUserService _nccUserService;
        
        public int Order { get { return 100; } }

        public NccAuthFilter(INccUserService nccUserService, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
            _nccUserService = nccUserService;            
            //_cache = memoryCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthorized = false;            
            var action = (ControllerActionDescriptor)context.ActionDescriptor;
            var actionAttributes = action.MethodInfo.GetCustomAttributes(true);
            var type = action.ControllerTypeInfo;
            var moduleName = type.Assembly.GetName().Name;
            var controllerAttributes = type.GetCustomAttributes(true);

            // Allow actions or controller whoich have AllowAnonymous attribute.
            if (actionAttributes.Where(x => x is AllowAnonymousAttribute).Count() > 0)
            {
                return;
            }

            if (controllerAttributes.Where(x => x is AllowAnonymousAttribute).Count() > 0)
            {
                if (
                    actionAttributes.Where(x => x is NccAuthorize).Count() == 0 
                    && actionAttributes.Where(x => x is AdminMenuItem).Count() == 0 
                    && actionAttributes.Where(x => x is SiteMenuItem).Count() == 0 
                    && actionAttributes.Where(x => x is SubActionOf).Count() == 0 )
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

            var nccUser = _nccUserService.Get(user.GetUserId());

            if (nccUser == null)
            {
                context.Result = new ChallengeResult(new AuthenticationProperties());
                context.HttpContext.Items["ErrorMessage"] = "No user found.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
                return;
            }

            if (nccUser.IsRequireLogin)
            {
                SignInManager<NccUser> signInManager = (SignInManager<NccUser>) context.HttpContext.RequestServices.GetService(typeof(SignInManager<NccUser>));
                signInManager.SignOutAsync().Wait();
                context.HttpContext.SignOutAsync().Wait();
                GlobalContext.GlobalCache.RemoveNccUserFromCache(nccUser.Id);
                context.Result = new ChallengeResult(new AuthenticationProperties());
                context.HttpContext.Items["ErrorMessage"] = "You do not have enought permission.";
                context.HttpContext.Response.Redirect("/Account/Login");
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
                    (notFound, isRedirect, isAuthorized) = IsAuthorized(nccUser, moduleName, subActionOf.Controller, subActionOf.Action);
                    if (isAuthorized)
                        break;
                }
            }

            if(isAuthorized == false)
            {
                (notFound, isRedirect, isAuthorized) = IsAuthorized(nccUser, moduleName, action.ControllerName, action.ActionName);
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
                return;
            }
        }

        private (bool NotFound, bool Redirect, bool isAuthorized) IsAuthorized(NccUser nccUser, string moduleName, string controllerName, string actionName)
        {
            bool isAuthorized = false;
            string menuName = "";

            (List<ControllerAction> caList, bool isMainAction) = GetControllerActionForThisRequest(controllerName, actionName);

            if (isMainAction)
            {
                foreach (var item in caList)
                {
                    if (string.IsNullOrEmpty(item.MainMenuName) == true || string.IsNullOrEmpty(item.MainController) == true || string.IsNullOrEmpty(item.MainAction) == true)
                    {
                        return (true, true, false);
                    }

                    if (nccUser.ExtraDenies.Where(x => x.ModuleName == item.ModuleName && x.Action == item.MainAction && x.Controller == item.MainController).Count() > 0)
                    {
                        return (false, true, false);
                    }
                    else
                    {
                        if (nccUser.Permissions.Where(x => x.Permission.PermissionDetails.Where(y => y.ModuleName == item.ModuleName && y.Action == item.MainAction && y.Controller == item.MainController).Count() > 0).Count() > 0)
                        {
                            isAuthorized = true;
                        }
                        else
                        {
                            isAuthorized = nccUser.ExtraPermissions.Where(x => x.ModuleName == item.ModuleName && x.Action == item.MainAction && x.Controller == item.MainController).Count() > 0;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in caList)
                {
                    
                    if (nccUser.Permissions.Where(x => x.Permission.PermissionDetails.Where(y => y.ModuleName == item.ModuleName && y.Action == item.MainAction && y.Controller == item.MainController).Count() > 0).Count() > 0)
                    {
                        isAuthorized = true;
                    }
                    
                    if(isAuthorized == false)
                    {
                        isAuthorized = nccUser.ExtraPermissions.Where(x => x.ModuleName == item.ModuleName && x.Action == item.MainAction && x.Controller == item.MainController).Count() > 0;
                    }

                    if (isAuthorized)
                    {
                        break;
                    }
                }
            }
                        
            return (false, false, isAuthorized);
        }

        private (List<ControllerAction> controllerActions, bool isMainAction) GetControllerActionForThisRequest(string controllerName, string actionName)
        {
            bool isMainAction = ControllerActionCache.ControllerActions.Where(x => x.MainAction == actionName && x.MainAction == controllerName).Count() > 0;
            var caList = ControllerActionCache.ControllerActions.Where(x => x.SubAction == actionName && x.SubController == controllerName).ToList();
            if (caList != null && caList.Count > 0)
            {
                return (caList, isMainAction);
            }
            return (new List<ControllerAction>(), isMainAction);
        }
        
        //private string GetModuleId(TypeInfo type)
        //{
        //    var assemblyName = type.Assembly.GetName();
        //    var module = GlobalContext.GetModuleByAssemblyName(assemblyName);
        //    if (module != null)
        //    {
        //        return module.ModuleId;
        //    }
        //    return null;
        //}
    }
}
