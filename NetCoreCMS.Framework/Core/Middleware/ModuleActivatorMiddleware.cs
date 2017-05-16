using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreCMS.Framework.Core.Middleware
{
    public static class ModuleActivatorMiddleware
    {
        public static IApplicationBuilder UseModuleActivator(this IApplicationBuilder app, IHostingEnvironment env, IMvcBuilder mvcBuilder, IServiceCollection services, ILoggerFactory loggerFactory)
        {
            return app.Use((context, next) =>
            {
                services.Configure<RazorViewEngineOptions>(options =>
                {
                    options.ViewLocationExpanders.Add(new ModuleViewLocationExpendar());
                });

                

                //mvcBuilder.AddRazorOptions(o =>
                //{
                //    foreach (var module in modules)
                //    {
                //        o.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(module.Assembly.Location));
                //    }
                //});
                return next();
            });
            
        }
    }
}
