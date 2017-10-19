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
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Middleware
{
    public class CultureDetectorMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        string _currentLanguage = "";
        public CultureDetectorMiddleware(RequestDelegate next, ILogger<MaintenanceMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var feature = context.Features.Get<IRequestCultureFeature>();
            _currentLanguage = feature.RequestCulture.Culture.Name.ToLower();
            context.Session.SetString(SupportedCultures.LanguageSessionKey, _currentLanguage);
            context.Response.Cookies.Append(SupportedCultures.LanguageCookieKey, _currentLanguage);
            await next.Invoke(context); 
        }
    }
     
    public static class CultureDetactorMiddlewareExtensions
    {
        //public static IServiceCollection AddCultureDetactor(this IServiceCollection services)
        //{
        //    return services;
        //}
        
        public static IApplicationBuilder UseCultureDetactor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CultureDetectorMiddleware>();
        }
    }
}
