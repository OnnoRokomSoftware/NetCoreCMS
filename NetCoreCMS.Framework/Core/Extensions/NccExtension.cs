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

namespace NetCoreCMS.Framework.Core.Extensions
{
    public static class NccExtension
    {
        public static IServiceCollection AddNccCoreModuleServices(this IServiceCollection services)
        {

            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<SignInManager<NccUser>, NccSignInManager<NccUser>>();
            services.AddScoped<IViewRenderService, NccRazorViewRenderService>();
            services.AddTransient<NccLanguageDetector>();

            services.AddScoped<NccLanguageFilter>();
            services.AddScoped<NccGlobalExceptionFilter>();
            services.AddScoped<NccAuthFilter>();

            services.AddScoped<LanguageEnabledAnchorTagHelper, LanguageEnabledAnchorTagHelper>();
            services.AddScoped<NccShortCodeProvider, NccShortCodeProvider>();
            services.AddScoped<ThemeManager, ThemeManager>();
            services.AddScoped<NccRazorViewRenderService, NccRazorViewRenderService>();

            services.AddTransient<NccCategoryDetailsRepository>();
            services.AddTransient<NccCategoryDetailsService>();
            services.AddTransient<NccPageDetailsRepository>();
            services.AddTransient<NccPageDetailsService>();
            services.AddTransient<NccPostDetailsRepository>();
            services.AddTransient<NccPostDetailsService>();
            services.AddTransient<NccTagRepository>();
            services.AddTransient<NccTagService>();

            services.AddTransient<NccSettingsRepository>();
            services.AddTransient<NccSettingsService>();
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
            
            return null;
        }
    }
}
