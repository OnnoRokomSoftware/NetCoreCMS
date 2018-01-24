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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Middleware;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Views;
using NetCoreCMS.Framework.Core.Mvc.Views.TagHelpers;
using NetCoreCMS.Framework.Core.Repository;
using NetCoreCMS.Framework.Core.Services;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Resources;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Mvc.Filters;
using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using NetCoreCMS.Framework.Core.Mvc.Provider;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using MediatR;
using NetCoreCMS.Framework.Modules;

namespace NetCoreCMS.Framework.Core.Extensions
{
    public static class NccExtension
    {
        public static IServiceCollection AddNccCoreModuleServices(this IServiceCollection services)
        { 
            services.AddScoped<INccTranslator, NccTranslator>();

            services.AddScoped<SignInManager<NccUser>, NccSignInManager<NccUser>>();
            services.AddScoped<IViewRenderService, NccRazorViewRenderService>();
            services.AddScoped<NccLanguageDetector>();

            services.AddSingleton<IMediator,Mediator>();

            services.AddScoped<NccAuthFilter>();
            services.AddScoped<NccControllerFilter>();
            services.AddScoped<NccDataAuthFilter>();
            services.AddScoped<NccGlobalExceptionFilter>();
            services.AddScoped<NccLanguageFilter>();
            services.AddScoped<NccLoggerFilter>();

            services.AddScoped<LanguageEnabledAnchorTagHelper, LanguageEnabledAnchorTagHelper>();
            services.AddScoped<NccShortCodeProvider, NccShortCodeProvider>();
            services.AddScoped<ThemeManager, ThemeManager>();
            services.AddScoped<NccRazorViewRenderService, NccRazorViewRenderService>();

            services.AddScoped<NccCategoryDetailsRepository>();
            services.AddScoped<NccCategoryDetailsService>();
            services.AddScoped<NccPageDetailsRepository>();
            services.AddScoped<NccPageDetailsService>();

            services.AddScoped<NccPageRepository>();
            services.AddScoped<NccPageService>();
            services.AddScoped<NccCategoryRepository>();
            services.AddScoped<NccCategoryService>();

            services.AddScoped<NccUserRepository>();
            services.AddScoped<INccUserService, NccUserService>();

            services.AddScoped<NccPostRepository>();
            services.AddScoped<NccPostService>();
            services.AddScoped<NccPostDetailsRepository>();
            services.AddScoped<NccPostDetailsService>();
            services.AddScoped<NccTagRepository>();
            services.AddScoped<NccTagService>();
            services.AddScoped<NccCommentsRepository>();
            services.AddScoped<NccCommentsService>();

            services.AddScoped<NccSettingsRepository>();
            services.AddScoped<INccSettingsService, NccSettingsService>();
            services.AddScoped<NccMenuRepository>();
            services.AddScoped<NccMenuService>();
            services.AddScoped<NccMenuRepository>();
            services.AddScoped<NccMenuItemRepository>();
            services.AddScoped<NccModuleRepository>();
            services.AddScoped<NccModuleService>();
            
            services.AddScoped<NccWebSiteWidgetRepository>();
            services.AddScoped<NccWebSiteWidgetService>();

            services.AddScoped<NccWebSiteRepository>();
            services.AddScoped<NccWebSiteInfoRepository>();
            services.AddScoped<NccWebSiteService>();
            services.AddScoped<NccStartupRepository>();
            services.AddScoped<NccStartupService>();

            services.AddScoped<NccPermissionRepository>();
            services.AddScoped<NccPermissionService>();
            services.AddScoped<NccPermissionDetailsRepository>();
            services.AddScoped<NccPermissionDetailsService>();

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
