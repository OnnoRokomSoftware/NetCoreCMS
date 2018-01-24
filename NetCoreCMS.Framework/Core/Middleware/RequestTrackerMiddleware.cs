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
using System.Diagnostics;

namespace NetCoreCMS.Framework.Core.Middleware
{
    public class RequestTrackerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        
        public RequestTrackerMiddleware(RequestDelegate next, IMediator mediator, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestTrackerMiddleware>();
            _mediator = mediator;            
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.HttpContext.Response.Headers["X-Framework"] =  NccInfo.Name;
            context.Request.HttpContext.Response.Headers["X-Framework-Version"] = NccInfo.Version.ToString(4);
            var sw = new Stopwatch();
            sw.Start();
            FireEvent(AppActivity.Type.RequestStart, context, 0);            
            await _next.Invoke(context);
            sw.Stop();
            FireEvent(AppActivity.Type.RequestEnd, context, sw.ElapsedMilliseconds);
            _logger.LogInformation($"[Path:{context.Request.Path.Value} | Response Time: {sw.ElapsedMilliseconds} ms]");             
        }

        private void FireEvent(AppActivity.Type type, HttpContext context, long responseTime)
        {
            try
            {
                _mediator.SendAll(new OnAppActivity(new AppActivity()
                {
                    ActivityType = type,
                    Context = context,
                    ResponseTime = responseTime,
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
