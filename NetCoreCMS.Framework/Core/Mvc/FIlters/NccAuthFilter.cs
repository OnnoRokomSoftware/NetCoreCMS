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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Attributes;

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
            var isAuthorized = false;
            var type = context.Controller.GetType();
            var ctrl = (Controller)context.Controller;

            var attribs = type.GetCustomAttributes(true);

            foreach (var item in attribs)
            {
                if (item is AllowAnonymousAttribute)
                {
                    isAuthorized = true; 
                }

                if (item is NccAuthorize)
                {

                }
            }

            if(isAuthorized == false)
            {
                context.HttpContext.Items["Message"] = "You have not enough permission.";
                context.HttpContext.Response.Redirect("/Home/NotAuthorized");
            }
        }
    }
}
