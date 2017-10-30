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

namespace NetCoreCMS.Framework.Core.Mvc.FIlters
{
    public class NccGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public NccGlobalExceptionFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<NccGlobalExceptionFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Global Exception Filter");
        }
    }
}
