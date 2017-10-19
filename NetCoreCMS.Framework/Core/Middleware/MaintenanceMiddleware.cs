/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Middleware
{
    public class MaintenanceMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private readonly MaintenanceWindow window;

        public MaintenanceMiddleware(RequestDelegate next, MaintenanceWindow window, ILogger<MaintenanceMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
            this.window = window;
        }

        public async Task Invoke(HttpContext context)
        {
            var config = SetupHelper.LoadSetup();            
            if (config.IsMaintenanceMode)
            {
                if(context.User.IsInRole(NccCmsRoles.SuperAdmin) || context.Request.Path.Value.ToLower() == "/login" || context.Request.Path.Value.ToLower() == "/admin" || context.Request.Path.Value.ToLower() == "/account/login")
                {
                    await next.Invoke(context);
                }
                else
                {
                    // set the code to 503 for SEO reasons
                    context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    context.Response.Headers.Add("Retry-After", window.RetryAfterInSeconds.ToString());

                    context.Response.ContentType = window.ContentType;
                    await context
                        .Response
                        .WriteAsync(Encoding.UTF8.GetString(window.Response), Encoding.UTF8);
                }
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }

    public class MaintenanceWindow
    {

        private Func<bool> enabledFunc;
        private byte[] response;

        public MaintenanceWindow(Func<bool> enabledFunc, byte[] response)
        {
            this.enabledFunc = enabledFunc;
            this.response = response;
        }

        public bool Enabled => enabledFunc();
        public byte[] Response => response;

        public int RetryAfterInSeconds { get; set; } = 3600;
        public string ContentType { get; set; } = "text/html";
    }

    public static class MaintenanceWindowExtensions
    {
        public static IServiceCollection AddMaintenance(this IServiceCollection services, MaintenanceWindow window)
        {
            services.AddSingleton(window);
            return services;
        }

        public static IServiceCollection AddMaintenance(this IServiceCollection services, Func<bool> enabler, byte[] response, string contentType = "text/html", int retryAfterInSeconds = 3600)
        {
            AddMaintenance(services, new MaintenanceWindow(enabler, response)
            {
                ContentType = contentType,
                RetryAfterInSeconds = retryAfterInSeconds
            });

            return services;
        }

        public static IApplicationBuilder UseMaintenance(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MaintenanceMiddleware>();
        }
    }
}
