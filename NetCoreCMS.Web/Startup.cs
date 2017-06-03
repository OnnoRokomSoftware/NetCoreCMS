/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Services.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using NetCoreCMS.Framework.Themes;
using System.IO; 
using NetCoreCMS.Framework.Core.Services;
using System.Linq;
using NetCoreCMS.Framework.Core.Repository;

namespace NetCoreCMS.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        ModuleManager _moduleManager;
        ThemeManager _themeManager;
        NetCoreStartup _startup;
        IMvcBuilder _mvcBuilder;
        IServiceCollection _services;
        
        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            
            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }
            
            builder.AddEnvironmentVariables();            
            Configuration = builder.Build();
            
            GlobalConfig.ContentRootPath = env.ContentRootPath;
            GlobalConfig.WebRootPath = env.WebRootPath;

            _moduleManager = new ModuleManager();            
            var setupConfig = SetupHelper.LoadSetup();
            _startup = new NetCoreStartup();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _mvcBuilder = services.AddMvc();
            _services = services;

            _services.AddSession();
            _services.AddDistributedMemoryCache();
            
            if (SetupHelper.IsDbCreateComplete)
            {
                _services.AddDbContext<NccDbContext>(options =>
                    options.UseSqlite(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework"))
                );

                _services.AddCustomizedIdentity();
                // Add application services.
                _startup.RegisterDatabase(_services);
            }

            _services.AddTransient<IEmailSender, AuthMessageSender>();
            _services.AddTransient<ISmsSender, AuthMessageSender>();
            _services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            _services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            _services.AddScoped<SignInManager<NccUser>, NccSignInManager<NccUser>>();
            _services.AddTransient<NccWebSiteRepository>();
            _services.AddTransient<NccWebSiteWidgetRepository>();
            _services.AddTransient<NccWebSiteService>();
            _services.AddTransient<NccWebSiteWidgetService>();
            _services.AddTransient<NccMenuRepository>();
            _services.AddTransient<NccMenuItemRepository>();
            _services.AddTransient<NccMenuService>();
            
            var moduleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);
            var themesFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ThemeFolder);

            _moduleManager.LoadModules(coreModuleFolder);
            _moduleManager.LoadModules(moduleFolder);
            GlobalConfig.Modules = _moduleManager.RegisterModules(_mvcBuilder, _services);
            ThemeManager.RegisterThemes(_mvcBuilder, _services, themesFolder);

            _services.AddSingleton<IConfiguration>(Configuration);
            _services.AddSingleton<IConfigurationRoot>(Configuration);

            var serviceProvider = _services.Build(Configuration, _hostingEnvironment);

            GlobalConfig.Services = _services;
            return serviceProvider;
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, NccWebSiteWidgetService nccWebsiteWidgetServices, NccWebSiteService nccWebsiteService, NccMenuService menuService)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            _themeManager = new ThemeManager(loggerFactory);
            var themeFolder = Path.Combine(env.ContentRootPath, NccInfo.ThemeFolder);
            GlobalConfig.Themes = _themeManager.ScanThemeDirectory(themeFolder);
            
            ResourcePathExpendar.RegisterStaticFiles(env, app, GlobalConfig.Modules, GlobalConfig.Themes);

            //app.UseThemeActivator(env, loggerFactory);
            //app.UseModuleActivator(env, _mvcBuilder, _services, loggerFactory);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            
            if (SetupHelper.IsDbCreateComplete)
            {
                app.UseIdentity();
            }
            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=CmsHome}/{action=Index}/{id?}");
            });
            
            GlobalConfig.App = app;
            GlobalConfig.WebSite = nccWebsiteService.LoadAll().FirstOrDefault();
            GlobalConfig.WebSiteWidgets = nccWebsiteWidgetServices.LoadAll();
            GlobalConfig.ListWidgets();
            GlobalConfig.Menus = menuService.LoadAllSiteMenus();
        }
    }
}
