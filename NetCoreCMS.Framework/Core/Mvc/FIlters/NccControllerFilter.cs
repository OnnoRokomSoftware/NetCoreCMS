using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Services;

namespace NetCoreCMS.Framework.Core.Mvc.Filters
{
    public class NccControllerFilter : IActionFilter
    {
        public int Order { get; set; }
        
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMemoryCache _memoryCache;
        private readonly INccUserService _nccUserService;
        private readonly INccSettingsService _nccSettingsService;

        public NccControllerFilter(ILoggerFactory loggerFactory, IMemoryCache memoryCache, INccUserService nccUserService, INccSettingsService  nccSettingsService)
        {
            _loggerFactory = loggerFactory;
            _memoryCache = memoryCache;
            _nccUserService = nccUserService;
            _nccSettingsService = nccSettingsService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Items.ContainsKey("NCC_CONTROLLER_PROPERTY_SETTINGS"))
            {
                context.HttpContext.Items["NCC_CONTROLLER_PROPERTY_SETTINGS"] = _nccSettingsService;
            }
            else
            {
                context.HttpContext.Items.Add("NCC_CONTROLLER_PROPERTY_SETTINGS", _nccSettingsService);
            }

            if (context.HttpContext.Items.ContainsKey("NCC_CONTROLLER_PROPERTY_USER_SERVICE"))
            {
                context.HttpContext.Items["NCC_CONTROLLER_PROPERTY_USER_SERVICE"] = _nccUserService;
            }
            else
            {
                context.HttpContext.Items.Add("NCC_CONTROLLER_PROPERTY_USER_SERVICE", _nccUserService);
            }

            if (context.HttpContext.Items.ContainsKey("NCC_CONTROLLER_PROPERTY_CACHE"))
            {
                context.HttpContext.Items["NCC_CONTROLLER_PROPERTY_CACHE"] = _memoryCache;
            }
            else
            {
                context.HttpContext.Items.Add("NCC_CONTROLLER_PROPERTY_CACHE", _memoryCache);
            }
            
            if (context.HttpContext.Items.ContainsKey("NCC_RAZOR_PAGE_PROPERTY_CONTROLLER_NAME"))
            {
                var action = (ControllerActionDescriptor)context.ActionDescriptor;                
                var controller = action.ControllerName;
                context.HttpContext.Items["NCC_RAZOR_PAGE_PROPERTY_CONTROLLER_NAME"] = controller;
            }
            else
            {
                var action = (ControllerActionDescriptor)context.ActionDescriptor;
                var controller = action.ControllerName;
                context.HttpContext.Items.Add("NCC_RAZOR_PAGE_PROPERTY_CONTROLLER_NAME", controller);
            }

            //var returnUrl = WebUtility.UrlDecode(context.HttpContext.Request.Path);
            var returnUrl = context.HttpContext.Request.Path.Value;
            if (context.HttpContext.Request.QueryString.HasValue)
            {
                var qStr = context.HttpContext.Request.QueryString.Value;
                returnUrl = returnUrl + qStr;
            }

            if (context.HttpContext.Items.ContainsKey("NCC_RAZOR_PAGE_PROPERTY_PAGE_URL"))
            { 
                context.HttpContext.Items["NCC_RAZOR_PAGE_PROPERTY_PAGE_URL"] = returnUrl;
            }
            else
            { 
                context.HttpContext.Items.Add("NCC_RAZOR_PAGE_PROPERTY_PAGE_URL", returnUrl);
            }

        }
    }
}
