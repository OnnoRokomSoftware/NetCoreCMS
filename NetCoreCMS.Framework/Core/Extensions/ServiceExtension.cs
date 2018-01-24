/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Auth;
using Microsoft.CodeAnalysis;
using NetCoreCMS.Framework.Core.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Twitter;

namespace NetCoreCMS.Framework.Core.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services, INccSettingsService nccSettingsService)
        {
            services.AddIdentity<NccUser, NccRole>(
                configure => {                
                    
                    if (GlobalContext.HostingEnvironment.IsDevelopment())
                    {
                        configure.Password.RequiredLength = 1;
                        configure.Password.RequireDigit = false;
                        configure.Password.RequireLowercase = false;
                        configure.Password.RequireNonAlphanumeric = false;
                        configure.Password.RequireUppercase = false;
                    }
                    else
                    {
                        configure.Password.RequiredLength = 6;
                        configure.Password.RequireDigit = true;
                        configure.Password.RequireLowercase = false;
                        configure.Password.RequireNonAlphanumeric = false;
                        configure.Password.RequireUppercase = false;                         
                    }

                    configure.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(365 * 200);
                    configure.Lockout.MaxFailedAccessAttempts = 5;

                    configure.SignIn.RequireConfirmedEmail = false;
                    configure.SignIn.RequireConfirmedPhoneNumber = false;                    
                }
            )
            .AddEntityFrameworkStores<NccDbContext>()
            .AddRoleStore<NccRoleStore>()
            .AddUserStore<NccUserStore>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<NccUser>>();

            var authBuilder = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            authBuilder = authBuilder.AddCookie(options => {
                options.Cookie.Name = ".NetCoreCMS.Cookie";
                options.Cookie.Expiration = new TimeSpan(0, 20, 0);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logoff";
                options.AccessDeniedPath = "/Account/AccessDenied";                     
            });

            if (nccSettingsService != null)
            {
                var settings = nccSettingsService.GetByKey<OpenIdSettings>();
                
                if (settings != null && string.IsNullOrEmpty(settings.GoogleClientId) == false && string.IsNullOrEmpty(settings.GoogleClientSecret) == false)
                {
                    authBuilder = authBuilder.AddGoogle(GoogleDefaults.AuthenticationScheme, x =>
                    {
                        x.ClientId = settings.GoogleClientId;
                        x.ClientSecret = settings.GoogleClientSecret;
                        x.CallbackPath = "/Account/ExternalLoginCallback";                        
                        x.Events = new OAuthEvents
                        {
                            OnRemoteFailure = ctx => HandleRemoteLoginFailure(ctx)
                        };
                    });
                } 
                
                if(settings != null && string.IsNullOrEmpty(settings.MicrosoftAppId) == false && string.IsNullOrEmpty(settings.MicrosoftAppPassword) == false)
                {
                    authBuilder = authBuilder.AddMicrosoftAccount(x =>
                     {
                         x.ClientId = settings.MicrosoftAppId;
                         x.ClientSecret = settings.MicrosoftAppPassword;
                         x.CallbackPath = "/Account/ExternalLoginCallback";
                     });
                }

                if(settings != null && string.IsNullOrEmpty(settings.TwitterConsumerKey) == false && string.IsNullOrEmpty(settings.TwitterCustomerSecret) == false)
                {
                    authBuilder = authBuilder.AddTwitter(TwitterDefaults.AuthenticationScheme, x =>
                     {
                         x.CallbackPath = "/Account/ExternalLoginCallback";
                         x.ConsumerKey = settings.TwitterConsumerKey;
                         x.ConsumerSecret = settings.TwitterCustomerSecret;
                     });
                }

                if (settings != null && string.IsNullOrEmpty(settings.FacebookAppId) == false && string.IsNullOrEmpty(settings.FacebookAppSecret) == false)
                {
                    authBuilder = authBuilder.AddFacebook(FacebookDefaults.AuthenticationScheme, x =>
                    {
                        x.AppId = settings.FacebookAppId;
                        x.AppSecret = settings.FacebookAppSecret;
                        x.CallbackPath = "/Account/ExternalLoginCallback";

                        x.Scope.Add("email");
                        x.Fields.Add("name");
                        x.Fields.Add("email");
                        x.SaveTokens = true;

                        x.Events = new OAuthEvents
                        {
                            OnRemoteFailure = ctx => HandleRemoteLoginFailure(ctx)
                        };
                    });
                }
            }
            
            services.ConfigureApplicationCookie(options => { options.LoginPath = "/Account/Login"; });

            /*
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => o.LoginPath = new PathString("/Account/login"))

                .AddFacebook(x =>
                {
                    x.AppId = "";
                    x.AppSecret = "";

                    x.Events = new OAuthEvents
                    {
                        OnRemoteFailure = ctx => HandleRemoteLoginFailure(ctx)
                    };
                })
                .AddGoogle(x =>
                {
                    x.ClientId = "";
                    x.ClientSecret = "";
                    x.Events = new OAuthEvents
                    {
                        OnRemoteFailure = ctx => HandleRemoteLoginFailure(ctx)
                    };
                });
            */
            
            return services;
        }

        private static Task HandleRemoteLoginFailure(RemoteFailureContext ctx)
        {
            ctx.HttpContext.Items["ErrorMessage"] = ctx.Failure.Message;
            ctx.Response.Redirect("/Account/Login");
            ctx.HandleResponse();
            return Task.CompletedTask;
        }

        public static IServiceProvider Build(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment hostingEnvironment)
        {
            var builder = new ContainerBuilder();            
            builder.RegisterInstance(configuration);
            builder.RegisterInstance(hostingEnvironment);
            builder.Populate(services);
            var container = builder.Build();            
            return container.Resolve<IServiceProvider>();
        }

        public static IServiceProvider BuildModules(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment hostingEnvironment)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });

            //Added into MediatR because default handlers are decleard at NetCoreCMS.Framework library.
            var coreFrameworkAssembly = typeof(NccInfo).Assembly;
            builder.RegisterAssemblyTypes(coreFrameworkAssembly).PropertiesAutowired().AsImplementedInterfaces();
            services.AddMediatR(coreFrameworkAssembly);

            foreach (var module in GlobalContext.GetActiveModules())
            {
                builder.RegisterAssemblyTypes(module.Assembly).PropertiesAutowired().AsImplementedInterfaces();
                services.AddMediatR(module.Assembly);
            }

            builder.RegisterInstance(configuration);
            builder.RegisterInstance(hostingEnvironment);
            builder.Populate(services);
            var container = builder.Build();

            return container.Resolve<IServiceProvider>();
        } 
    }
}
