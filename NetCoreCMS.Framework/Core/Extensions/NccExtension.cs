/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Middleware;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Mvc.Views.TagHelpers;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.Services.Auth;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Filters;
using NetCoreCMS.Framework.Modules.Loader;
using Microsoft.Extensions.DependencyModel;
using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NetCoreCMS.Framework.Core.Mvc.Provider;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using MediatR;

namespace NetCoreCMS.Framework.Core.Extensions
{
    public static class NccExtension
    {
        public static IServiceCollection AddNccCoreModuleServices(this IServiceCollection services)
        { 
            services.AddScoped<INccTranslator, NccTranslator>();

            services.AddScoped<SignInManager<NccUser>, NccSignInManager<NccUser>>();
            services.AddScoped<IViewRenderService, NccRazorViewRenderService>();
            services.AddTransient<NccLanguageDetector>();

            services.AddSingleton<IMediator,Mediator>();

            services.AddScoped<NccLanguageFilter>();
            services.AddScoped<NccGlobalExceptionFilter>();
            services.AddScoped<NccDataAuthFilter>();

            services.AddScoped<LanguageEnabledAnchorTagHelper, LanguageEnabledAnchorTagHelper>();
            services.AddScoped<NccShortCodeProvider, NccShortCodeProvider>();
            services.AddScoped<ThemeManager, ThemeManager>();
            services.AddScoped<NccRazorViewRenderService, NccRazorViewRenderService>();

            services.AddTransient<NccCategoryDetailsRepository>();
            services.AddTransient<NccCategoryDetailsService>();
            services.AddTransient<NccPageDetailsRepository>();
            services.AddTransient<NccPageDetailsService>();

            services.AddTransient<NccPageRepository>();
            services.AddTransient<NccPageService>();
            services.AddTransient<NccCategoryRepository>();
            services.AddTransient<NccCategoryService>();

            services.AddTransient<NccUserRepository>();
            services.AddTransient<NccUserService>();

            services.AddTransient<NccPostRepository>();
            services.AddTransient<NccPostService>();
            services.AddTransient<NccPostDetailsRepository>();
            services.AddTransient<NccPostDetailsService>();
            services.AddTransient<NccTagRepository>();
            services.AddTransient<NccTagService>();
            services.AddTransient<NccCommentsRepository>();
            services.AddTransient<NccCommentsService>();

            services.AddTransient<NccSettingsRepository>();
            services.AddTransient<INccSettingsService, NccSettingsService>();
            services.AddTransient<NccMenuRepository>();
            services.AddTransient<NccMenuService>();
            services.AddTransient<NccMenuRepository>();
            services.AddTransient<NccMenuItemRepository>();
            services.AddTransient<NccModuleRepository>();
            services.AddTransient<NccModuleService>();
            
            services.AddTransient<NccWebSiteWidgetRepository>();
            services.AddTransient<NccWebSiteWidgetService>();

            services.AddTransient<NccWebSiteRepository>();
            services.AddTransient<NccWebSiteInfoRepository>();
            services.AddTransient<NccWebSiteService>();
            services.AddTransient<NccStartupRepository>();
            services.AddTransient<NccStartupService>();

            services.AddTransient<NccPermissionRepository>();
            services.AddTransient<NccPermissionService>();
            services.AddTransient<NccPermissionDetailsRepository>();
            services.AddTransient<NccPermissionDetailsService>();

            services.TryAddEnumerable(ServiceDescriptor.Transient<IApplicationModelProvider, NccApplicationModelProvider>());

            return services;
        }
         
        public static IApplicationBuilder UseNetCoreCMS(this IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            ResourcePathExpendar.RegisterStaticFiles(env, app, GlobalContext.Modules, GlobalContext.Themes);

            app.UseResponseCompression();
            app.UseResponseCaching(); //Use this attrib for cache [ResponseCache(Duration = 20)]            
            app.UseSession();
            app.UseStaticFiles();
            
            GlobalContext.App = app;

            if (SetupHelper.IsDbCreateComplete)
            {
                app.UseAuthentication();

                NccWebSiteWidgetService nccWebsiteWidgetServices = serviceProvider.GetService<NccWebSiteWidgetService>();
                NccWebSiteService nccWebsiteService = serviceProvider.GetService<NccWebSiteService>();
                NccMenuService menuServic = serviceProvider.GetService<NccMenuService>();

                GlobalContext.WebSite = nccWebsiteService.LoadAll().FirstOrDefault();
                ThemeHelper.WebSite = GlobalContext.WebSite;
                GlobalContext.WebSiteWidgets = nccWebsiteWidgetServices.LoadAll();
                GlobalContext.Menus = menuServic.LoadAllSiteMenus();
            }

            app.UseMaintenance();

            if (SetupHelper.IsAdminCreateComplete)
            {
                app.UseRequestLocalization(new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture(SetupHelper.Language),
                    SupportedCultures = SupportedCultures.Cultures,
                    SupportedUICultures = SupportedCultures.Cultures
                });
            }
            
            return app;
        }

        public static IServiceCollection AddModuleDependencies(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddRazorOptions(options =>
            {
                Hashtable moduleDependencies = GlobalContext.GetModuleDependencies();
                foreach (ModuleDependedLibrary mdl in moduleDependencies.Values)
                {
                    foreach (var path in mdl.AssemblyPaths)
                    {
                        options.AdditionalCompilationReferences.Add(MetadataReference.CreateFromFile(path));
                    }
                }
            });

            return services;
        }

        public static IServiceCollection SetGlobalCache(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            GlobalContext.SetGlobalCache(serviceProvider);
            return services;
        }

        public static IServiceCollection AddResponseCacheingAndCompression(this IServiceCollection services, IServiceProvider serviceProvider)
        {
            services.AddResponseCaching();
            services.AddDistributedMemoryCache();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                //options.Providers.Add<BrotliCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = new[] {
                        "text/html; charset=utf-8",
                        "image/svg+xml",
                        "text/plain",
                        "text/css",
                        "application/javascript",
                        "text/html",
                        "application/xml",
                        "text/xml",
                        "application/json",
                        "text/json"
                    };
            });

            return services;
        }
    }
}
