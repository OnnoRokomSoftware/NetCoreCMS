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
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Mvc.Attributes;

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccAuthFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public NccAuthFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
             
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var type = context.Controller.GetType();
            var attribs = type.GetCustomAttributes(true);
            foreach (var item in attribs)
            {
                if(item is NccAuthorize)
                {

                }
            }
        }
    }
}
