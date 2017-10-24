/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Events.App;
using System.Threading.Tasks;
using System;

namespace NetCoreCMS.Framework.Core.Middleware
{
    public class RequestTrackerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        
        public RequestTrackerMiddleware(RequestDelegate next, IMediator mediator, ILogger<MaintenanceMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _mediator = mediator;            
        }

        public async Task Invoke(HttpContext context)
        {
            FireEvent(AppActivity.Type.RequestStart, context);
            await _next.Invoke(context);
            FireEvent(AppActivity.Type.RequestEnd, context);
        }

        private void FireEvent(AppActivity.Type type, HttpContext context)
        {
            try
            {
                _mediator.SendAll(new OnAppActivity(new AppActivity()
                {
                    ActivityType = type,
                    Context = context
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }
    }
     
    public static class RequestTrackerMiddlewareExtensions
    {        
        public static IApplicationBuilder UseRequestTracker(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTrackerMiddleware>();
        }
    }   
}
