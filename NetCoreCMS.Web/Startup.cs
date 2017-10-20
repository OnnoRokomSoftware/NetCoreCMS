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
using Serilog;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;

using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Auth;
using NetCoreCMS.Framework.Core.Middleware;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.Extensions;
using NetCoreCMS.Framework.Core.ShotCodes;
using NetCoreCMS.Framework.Core.Messages;

namespace NetCoreCMS.Web
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;
        ModuleManager               _moduleManager;
        ThemeManager                _themeManager;
        NccShortCodeProvider        _nccShortCodeProvider;
        NetCoreStartup              _startup;
        IMvcBuilder                 _mvcBuilder;
        SetupConfig                 _setupConfig;
        IServiceCollection          _services;
        IServiceProvider            _serviceProvider;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _hostingEnvironment = env;

            GlobalConfig.ContentRootPath    = env.ContentRootPath;
            GlobalConfig.WebRootPath        = env.WebRootPath;
            
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            ConfigurationRoot = builder.Build();

            _moduleManager  = new ModuleManager();
            _setupConfig    = SetupHelper.LoadSetup();
            _startup        = new NetCoreStartup();

            var logFilePath = NccInfo.LogFolder + "\\NccLogs-{Date}.log";
            Log.Logger      = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.RollingFile(logFilePath).CreateLogger();
            
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _services = services;

            _services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));
            
            _services.AddOptions();            
            //services.AddSingleton(typeof(IStringLocalizerFactory), typeof(ClassLibraryStringLocalizerFactory));
            services.AddSingleton(typeof(IStringLocalizer), typeof(NccStringLocalizer<SharedResource>));            

            _services.AddLocalization();

            _mvcBuilder = services.AddMvc();            
            _mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
            _mvcBuilder.AddDataAnnotationsLocalization(options => {
                options.DataAnnotationLocalizerProvider = (type, factory) => new NccStringLocalizer<SharedResource>(factory, new HttpContextAccessor());
            });

            _services.AddResponseCaching();
            _services.AddSession();
            _services.AddDistributedMemoryCache();
            _services.AddResponseCompression();            
            _services.AddSingleton(Configuration);

            _services.AddNccCoreModuleRepositoryAndServices();

            _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);

            _themeManager = new ThemeManager();
            var themeFolder = Path.Combine(_hostingEnvironment.ContentRootPath, NccInfo.ThemeFolder);
            GlobalConfig.Themes = _themeManager.ScanThemeDirectory(themeFolder);

            var themesDirectoryContents = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ThemeFolder);
            _themeManager.RegisterThemes(_mvcBuilder, _services,_serviceProvider, themesDirectoryContents);
            
            var moduleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
            var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);
            var coreModules = _moduleManager.LoadModules(coreModuleFolder);
            var userModules = _moduleManager.LoadModules(moduleFolder);

            GlobalConfig.Modules.AddRange(userModules);
            
            _services.AddMaintenance(() => _setupConfig.IsMaintenanceMode, Encoding.UTF8.GetBytes("<div style='width:100%;text-align:center; padding-top:10px;'><h1>" + _setupConfig.MaintenanceMessage + "</h1></div>"), "text/html", _setupConfig.MaintenanceDownTime * 60);

            if (SetupHelper.IsDbCreateComplete)
            {
                if (SetupHelper.SelectedDatabase == "SqLite")
                {
                    _services.AddDbContext<NccDbContext>(options =>
                        options.UseSqlite(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework"))
                    );
                }
                else if (SetupHelper.SelectedDatabase == "MSSQL")
                {
                    _services.AddDbContext<NccDbContext>(options =>
                        options.UseSqlServer(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework"))
                    );
                }
                else if (SetupHelper.SelectedDatabase == "MySql")
                {
                    _services.AddDbContext<NccDbContext>(options =>
                        options.UseMySql(SetupHelper.ConnectionString, opt => opt.MigrationsAssembly("NetCoreCMS.Framework"))
                    );
                }

                _services.AddCustomizedIdentity();                
                _startup.RegisterDatabase(_services);

                _services.AddAuthorization(options =>
                {
                    options.AddPolicy("SuperAdmin", policy => policy.Requirements.Add(new AuthRequire("SuperAdmin", "")));
                    options.AddPolicy("Administrator", policy => policy.Requirements.Add(new AuthRequire("Administrator", "")));
                    options.AddPolicy("Editor", policy => policy.Requirements.Add(new AuthRequire("Editor", "")));
                    options.AddPolicy("Author", policy => policy.Requirements.Add(new AuthRequire("Author", "")));
                    options.AddPolicy("Contributor", policy => policy.Requirements.Add(new AuthRequire("Contributor", "")));
                    options.AddPolicy("Subscriber", policy => policy.Requirements.Add(new AuthRequire("Subscriber", "")));
                });

                _services.AddSingleton<IAuthorizationHandler, AuthRequireHandler>();
                _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);

                GlobalConfig.Modules = _moduleManager.RegisterModules(_mvcBuilder, _services, _serviceProvider);
                _moduleManager.RegisterModuleRepositoryAndServices(_mvcBuilder, _services, _serviceProvider);

                _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);

                GlobalConfig.Widgets = _moduleManager.RegisterModuleWidgets(_mvcBuilder, _services, _serviceProvider);
                var themeWidgets = _themeManager.RegisterThemeWidgets(_mvcBuilder, _services, _serviceProvider, themesDirectoryContents);

                if(themeWidgets != null && themeWidgets.Count > 0)
                {
                    GlobalConfig.Widgets.AddRange(themeWidgets);
                }

                _nccShortCodeProvider = _serviceProvider.GetService<NccShortCodeProvider>();
                GlobalConfig.ShortCodes = _nccShortCodeProvider.ScanAndRegisterShortCodes(GlobalConfig.Modules);

            }

            var defaultCulture = new RequestCulture("en");

            if (SetupHelper.IsAdminCreateComplete)
            {
                GlobalConfig.SetupConfig = SetupHelper.LoadSetup();
                defaultCulture = new RequestCulture(GlobalConfig.SetupConfig.Language);
                GlobalMessageRegistry.LoadMessagesFromStorage();
            }

            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("lang", typeof(LanguageRouteConstraint));
            });

            _services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = SupportedCultures.Cultures;
                    opts.DefaultRequestCulture = defaultCulture;
                    opts.SupportedCultures = supportedCultures;
                    opts.SupportedUICultures = supportedCultures;

                    var provider = new RouteDataRequestCultureProvider();
                    provider.RouteDataStringKey = "lang";
                    provider.UIRouteDataStringKey = "lang";
                    provider.Options = opts;
                    opts.RequestCultureProviders = new[] { provider };
                }
            );
            
            services.Configure<ClassLibraryLocalizationOptions>(
                options => options.ResourcePaths = new Dictionary<string, string>
                {
                    { "NetCoreCMS.Framework", "i18n/Resources" },
                    { "NetCoreCMS.Web", "Resources" }
                }
            );

            _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);

            GlobalConfig.ServiceProvider = _serviceProvider;
            GlobalConfig.Services = _services;            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {    
            app.UseNetCoreCms(env, _serviceProvider, loggerFactory);
            app.UseNccRoutes(env, _serviceProvider, loggerFactory); 
        }
    }
}

