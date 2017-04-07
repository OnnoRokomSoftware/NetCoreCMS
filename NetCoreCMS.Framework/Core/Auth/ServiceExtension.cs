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
using System.Text;

namespace NetCoreCMS.Framework.Core.Auth
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCustomizedIdentity(this IServiceCollection services)
        {
            services.AddIdentity<NccUser, NccRole>(configure => {                
                configure.Password.RequireDigit = false;
                configure.Password.RequireLowercase = false;
                configure.Password.RequireNonAlphanumeric = false;
                configure.Password.RequireUppercase = false;
                configure.Password.RequiredLength = 1;
                }
            )
            .AddRoleStore<NccRoleStore>()
            .AddUserStore<NccUserStore>()
            .AddDefaultTokenProviders();
            return services;
        }

        public static IServiceProvider Build(this IServiceCollection services, IConfigurationRoot configuration, IHostingEnvironment hostingEnvironment)
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

            foreach (var module in GlobalConfig.Modules)
            {
                builder.RegisterAssemblyTypes(module.Assembly).AsImplementedInterfaces();
            }

            builder.RegisterInstance(configuration);
            builder.RegisterInstance(hostingEnvironment);
            builder.Populate(services);
            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }
    }
}
