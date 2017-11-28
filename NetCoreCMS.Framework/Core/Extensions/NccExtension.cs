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
using NetCoreCMS.Framework.Core.Mvc.FIlters;
using NetCoreCMS.Framework.Modules.Loader;
using Microsoft.Extensions.DependencyModel;
using System.Collections;
using Microsoft.CodeAnalysis;

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
            

            services.AddScoped<NccLanguageFilter>();
            services.AddScoped<NccGlobalExceptionFilter>();
            services.AddScoped<NccDataAuthFilter>();

            services.AddScoped<LanguageEnabledAnchorTagHelper, LanguageEnabledAnchorTagHelper>();
            services.AddScoped<NccShortCodeProvider, NccShortCodeProvider>();
            services.AddScoped<ThemeManager, ThemeManager>();
            services.AddScoped<NccRazorViewRenderService, NccRazorViewRenderService>();

            services.AddScoped<NccCategoryDetailsRepository>();
            services.AddScoped<NccCategoryDetailsService>();
            services.AddScoped<NccPageDetailsRepository>();
            services.AddScoped<NccPageDetailsService>();

            services.AddTransient<NccPageRepository>();
            services.AddTransient<NccPageService>();
            services.AddTransient<NccCategoryRepository>();
            services.AddTransient<NccCategoryService>();

            services.AddScoped<NccUserRepository>();
            services.AddScoped<NccUserService>();

            services.AddScoped<NccPostRepository>();
            services.AddScoped<NccPostService>();
            services.AddScoped<NccPostDetailsRepository>();
            services.AddScoped<NccPostDetailsService>();
            services.AddScoped<NccTagRepository>();
            services.AddScoped<NccTagService>();
            services.AddScoped<NccCommentsRepository>();
            services.AddScoped<NccCommentsService>();

            services.AddScoped<NccSettingsRepository>();
            services.AddScoped<NccSettingsService>();
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
            
            return services;
        }
         
        public static IApplicationBuilder UseNetCoreCMS(this IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            ResourcePathExpendar.RegisterStaticFiles(env, app, GlobalContext.Modules, GlobalContext.Themes);

            //app.UseThemeActivator(env, loggerFactory);
            //app.UseModuleActivator(env, _mvcBuilder, _services, loggerFactory);

            app.UseResponseCaching(); //Use this attrib for cache [ResponseCache(Duration = 20)]
            app.UseResponseCompression();
            app.UseSession();
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
    }
}
