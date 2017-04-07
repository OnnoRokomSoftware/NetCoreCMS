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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCoreCMS.Web.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;

namespace NetCoreCMS.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        ModuleManager _moduleManager;
        NetCoreStartup _startup;

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
            var setupConfig = SetupHelper.LoadSetup(env);
            _startup = new NetCoreStartup();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        { 
            services.AddSession();
            services.AddDistributedMemoryCache();

            if (SetupHelper.IsDbCreateComplete)
            {
                services.AddDbContext<NccDbContext>(options =>
                    options.UseSqlite(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework"))
                );
                services.AddCustomizedIdentity();
                
            }

            var mvcBuilder = services.AddMvc();

            var moduleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);

            _moduleManager.LoadModules(moduleFolder);
            _moduleManager.LoadModules(coreModuleFolder);
            GlobalConfig.Modules = _moduleManager.RegisterModules(mvcBuilder, services);
            GlobalConfig.Services = services;

            // Add application services.
            
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IConfigurationRoot>(Configuration);
            services.AddScoped<SignInManager<NccUser>, NccSignInManager<NccUser>>();
            
            _startup.RegisterDatabase(services);
            return services.Build(Configuration, _hostingEnvironment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            ResourcePathExpendar.RegisterStaticFiles(env, app, GlobalConfig.Modules);
            GlobalConfig.App = app;
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

        }
    }
}
