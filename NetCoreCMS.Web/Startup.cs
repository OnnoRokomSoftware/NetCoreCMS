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

using MediatR;

using Microsoft.AspNetCore.Builder;
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
using Microsoft.AspNetCore.Mvc.Authorization;

using NetCoreCMS.Framework.Resources;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Middleware;
using NetCoreCMS.Framework.i18n;
using NetCoreCMS.Framework.Core.Extensions;
using NetCoreCMS.Framework.Core.Messages;
using NetCoreCMS.Framework.Core.App;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetCoreCMS.Framework.Modules.Widgets;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Modules;
using NetCoreCMS.Framework.Core.Services.Auth;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using NetCoreCMS.Framework.Core.Mvc.Cache;
using System.Net;
using NetCoreCMS.Framework.Core.Services;
using System.Collections;

namespace NetCoreCMS.Web
{
    public class Startup
    {
        private IHostingEnvironment _hostingEnvironment;
        ModuleManager _moduleManager;
        ThemeManager _themeManager;
        NetCoreStartup _startup;
        IMvcBuilder _mvcBuilder;
        SetupConfig _setupConfig;
        IServiceCollection _services;
        IServiceProvider _serviceProvider;

        private const string ExceptionsOnStartup = "Startup";
        private const string ExceptionsOnConfigureServices = "ConfigureServices";
        private const string ExceptionsOnConfigure = "Configure";

        private readonly Dictionary<string, List<Exception>> _exceptions;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _exceptions = new Dictionary<string, List<Exception>>{
                { ExceptionsOnStartup, new List<Exception>() },
                { ExceptionsOnConfigureServices, new List<Exception>() },
                { ExceptionsOnConfigure, new List<Exception>() },
            };

            try
            {
                Configuration = configuration;
                _hostingEnvironment = env;

                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                ConfigurationRoot = builder.Build();
                ResetGlobalContext(configuration, env); 
                
                _moduleManager = new ModuleManager();
                _themeManager = new ThemeManager();

                _startup = new NetCoreStartup();
                _setupConfig = SetupHelper.LoadSetup();

                GlobalContext.HostingEnvironment = env;

                AddLogger();
            }
            catch (Exception ex)
            {
                _exceptions[ExceptionsOnStartup].Add(ex);
            }
        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationRoot { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                _services = services;

                _services.AddScoped<IEmailSender, AuthMessageSender>();
                _services.AddScoped<ISmsSender, AuthMessageSender>();
                _services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                _services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

                _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);

                _services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

                _services.AddOptions();
                _services.AddSingleton(typeof(IStringLocalizer), typeof(NccStringLocalizer<SharedResource>));
                _services.AddLocalization();

                _mvcBuilder = _services.AddMvc(config =>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                    config.CacheProfiles.Add(NccCacheProfile.Default,
                    new CacheProfile()
                    {
                        VaryByHeader = "User-Agent",
                        Duration = 300,
                        VaryByQueryKeys = new string[] { "id", "name", "pageNumber", "page", "pageSize", "model", "lang", "status", "sessionId", "requestId", "start", "slug", }
                    });
                });

                _mvcBuilder.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);
                _mvcBuilder.AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) => new NccStringLocalizer<SharedResource>(factory, new HttpContextAccessor());
                });
                
                _services.AddMaintenance(() => _setupConfig.IsMaintenanceMode, Encoding.UTF8.GetBytes("<div style='width:100%;text-align:center; padding-top:10px;'><h1>" + _setupConfig.MaintenanceMessage + "</h1></div>"), "text/html", _setupConfig.MaintenanceDownTime * 60);

                _services.AddSession(options =>
                {
                    options.Cookie.Name = ".NetCoreCMS.Cookie.Session";
                    options.IdleTimeout = TimeSpan.FromMinutes(20);
                    options.Cookie.HttpOnly = true;
                });

                _services.AddResponseCacheingAndCompression(_serviceProvider);

                _services.AddSingleton(Configuration);

                _services.AddNccCoreModuleServices();
                _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);

                _services.SetGlobalCache(_serviceProvider);

                var themeFolder = Path.Combine(_hostingEnvironment.ContentRootPath, NccInfo.ThemeFolder);
                var moduleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ModuleFolder);
                var coreModuleFolder = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.CoreModuleFolder);
                var themesDirectoryContents = _hostingEnvironment.ContentRootFileProvider.GetDirectoryContents(NccInfo.ThemeFolder);

                _themeManager.ScanThemeDirectory(themeFolder);
                _themeManager.RegisterThemes(_mvcBuilder, _services, _serviceProvider, themesDirectoryContents);

                var loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Startup>();

                _moduleManager.LoadModules(coreModuleFolder, logger);
                _moduleManager.LoadModules(moduleFolder, logger);

                _services.AddModuleDependencies(_mvcBuilder);

                if (SetupHelper.IsDbCreateComplete)
                {
                    _startup.SelectDatabase(_services);
                    _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);
                    _moduleManager.AddModulesAsApplicationPart(_mvcBuilder, _services, _serviceProvider, logger);

                    _moduleManager.AddModuleServices(_services, logger);
                    _moduleManager.AddModuleFilters(_services, logger);
                    _moduleManager.AddShortcodes(_services, logger);
                    _moduleManager.AddModuleWidgets(_services, logger);
                    _moduleManager.AddModuleAuthorizationHandlers(_services, logger);

                    _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);
                    
                    _services.AddCustomizedIdentity(_serviceProvider.GetService<INccSettingsService>());                    
                    _moduleManager.LoadModuleMenus(logger);
                }

                var defaultCulture = new RequestCulture("en");

                if (SetupHelper.IsAdminCreateComplete)
                {
                    GlobalContext.SetupConfig = SetupHelper.LoadSetup();
                    defaultCulture = new RequestCulture(GlobalContext.SetupConfig.Language);                    
                }

                _services.Configure<RouteOptions>(options =>
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

                _services.Configure<ClassLibraryLocalizationOptions>(
                    options => options.ResourcePaths = new Dictionary<string, string>
                    {
                    { "NetCoreCMS.Framework", "i18n/Resources" },
                    { "NetCoreCMS.Web", "Resources" }
                    }
                );

                _serviceProvider = _services.Build(ConfigurationRoot, _hostingEnvironment);
                _serviceProvider = _services.BuildModules(ConfigurationRoot, _hostingEnvironment);

                if (SetupHelper.IsDbCreateComplete)
                {
                    _themeManager.RegisterThemeWidgets(_mvcBuilder, _services, _serviceProvider, themesDirectoryContents);
                    _moduleManager.RegisterModuleWidgets(_mvcBuilder, _services, _serviceProvider, logger);
                    _moduleManager.RegisterModuleFilters(_mvcBuilder, _serviceProvider, logger);
                    _moduleManager.RegisterModuleShortCodes(_mvcBuilder, _serviceProvider);
                }

                GlobalContext.ServiceProvider = _serviceProvider;
                GlobalContext.Services = _services;
                NetCoreCmsHost.Mediator = _serviceProvider.GetService<IMediator>();
                NetCoreCmsHost.Services = _services;
                NetCoreCmsHost.ServiceProvider = _serviceProvider;
                GlobalMessageRegistry.LoadMessagesFromStorage();

            }
            catch (Exception ex)
            {
                _exceptions[ExceptionsOnConfigureServices].Add(ex);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var log = loggerFactory.CreateLogger<Startup>();
            
            try
            {
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

                NetCoreCmsHost.Logger = loggerFactory.CreateLogger<Startup>();
                NetCoreCmsHost.HttpContext = new HttpContextAccessor().HttpContext;

                app.UseRequestTracker();
                app.UseNetCoreCMS(env, _serviceProvider, loggerFactory);
                app.UseNccRoutes(env, _serviceProvider, loggerFactory);                
            }
            catch (Exception ex)
            {
                _exceptions[ExceptionsOnConfigure].Add(ex);                
            }

            foreach (var item in _exceptions)
            {
                try
                {
                    foreach (var ex in item.Value)
                    {
                        log.LogError(ex.Message, ex);
                    }
                }
                catch (Exception ex)
                {
                    //:(
                }
            }

            if (_exceptions.Any(p => p.Value.Any()))
            {
                app.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.ContentType = "text/html";

                      foreach (var ex in _exceptions)
                      {
                          foreach (var val in ex.Value)
                          {
                              var errorMessage = "<strong>------------------Begin------------------</strong>";
                              log.LogError($"{ex.Key}:::{val.Message}");
                              errorMessage += "<h5 style='color:red;'>" + val.Message + "</h5><hr/>";

                              if(env.IsProduction() == false)
                              {
                                  errorMessage += "Stack trace: " + val.StackTrace + "<br/>";
                              }

                              errorMessage += "<strong>------------------End------------------</strong><br/>";
                              await context.Response.WriteAsync(errorMessage).ConfigureAwait(false);
                          }
                      }
                  });
                return;
            }
        }

        private void ResetGlobalContext(IConfiguration configuration, IHostingEnvironment env)
        {
            GlobalContext.ContentRootPath = env.ContentRootPath;
            GlobalContext.WebRootPath = env.WebRootPath;
            GlobalContext.HostingEnvironment = env;
            GlobalContext.Configuration = configuration;
            GlobalContext.ConfigurationRoot = ConfigurationRoot;
            GlobalContext.Widgets = new List<Widget>();
            GlobalContext.Modules = new List<IModule>();
            GlobalContext.Menus = new List<NccMenu>();
            GlobalContext.Themes = new List<Theme>();
            GlobalContext.WidgetTypes = new Hashtable();
        }
        private void AddLogger()
        {
            if (_setupConfig.LoggingLevel != (int)LogLevel.None)
            {
                var logFilePath = NccInfo.LogFolder + "\\{Date}_NetCoreCMS_Logs.log";
                var logCfg = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithEnvironmentUserName()
                    .Enrich.WithProperty("Version", NccInfo.Version)
                    .WriteTo.RollingFile(
                        logFilePath,
                        shared: true,
                        fileSizeLimitBytes: 10485760,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [Ncc v{Version}] [{Level}] [{SourceContext}] # Message: {Message} {Properties} {NewLine}{Exception}",
                        flushToDiskInterval: new TimeSpan(0, 0, 30)
                    );
                switch (_setupConfig.LoggingLevel)
                {
                    case 5:
                        logCfg.MinimumLevel.Fatal();
                        break;
                    case 4:
                        logCfg.MinimumLevel.Error();
                        break;
                    case 3:
                        logCfg.MinimumLevel.Warning();
                        break;
                    case 2:
                        logCfg.MinimumLevel.Information();
                        break;
                    case 1:
                        logCfg.MinimumLevel.Debug();
                        break;
                    case 0:
                        logCfg.MinimumLevel.Verbose();
                        break;
                    default:
                        break;
                }
                Log.Logger = logCfg.CreateLogger();
            }
        }
    }
}