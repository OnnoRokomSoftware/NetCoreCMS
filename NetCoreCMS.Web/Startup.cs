using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Web.Data;
using NetCoreCMS.Web.Models;
using NetCoreCMS.Web.Services;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;

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

            _moduleManager = new ModuleManager();
            var setupConfig = SetupHelper.LoadSetup(env);
            _startup = new NetCoreStartup();
            GlobalConfig.ContentRootPath = env.ContentRootPath;
            GlobalConfig.WebRootPath = env.WebRootPath;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(SetupHelper.ConnectionString)
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                    })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var mvcBuilder = services.AddMvc();

            var moduleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);

            _moduleManager.LoadModules(moduleFolder);
            _moduleManager.LoadModules(coreModuleFolder);
            GlobalConfig.Modules = _moduleManager.RegisterModules(mvcBuilder, services);

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            _startup.RegisterDatabase(services);
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
            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=CmsHome}/{action=Index}/{id?}");
            });
        }
    }
}
