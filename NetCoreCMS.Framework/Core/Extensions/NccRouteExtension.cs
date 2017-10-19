/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;
using System;
 

namespace NetCoreCMS.Framework.Core.Extensions
{
    public static class NccRouteExtension
    {
        public static IApplicationBuilder UseNccRoutes(this IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            app.UseMvc(routes =>
            {
                if (SetupHelper.IsAdminCreateComplete && GlobalConfig.WebSite.IsMultiLangual)
                {
                    routes.MapRoute(
                        name: "MultiLangAnyControlerAction",
                        template: "{lang:lang}/{controller}/{action}/{id?}",
                        defaults: new { controller = "Home", action = "Index" }
                    );

                    routes.MapRoute(
                        name: "MultiLangCmsPage",
                        template: "{lang:lang}/{slug?}",
                        defaults: new { controller = "CmsPage", action = "Index" });

                    routes.MapRoute(
                        name: "MultiLangBlogPost",
                        template: "{lang:lang}/Post/{slug?}",
                        defaults: new { controller = "Post", action = "Index" });

                    routes.MapRoute(
                        name: "MultiLangBlogCategory",
                        template: "{lang:lang}/Category/{slug?}",
                        defaults: new { controller = "Category", action = "Index" });

                    routes.MapRoute(
                        name: "login",
                        template: "Login",
                        defaults: new { controller = "Account", action = "Login" }
                    );

                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                         defaults: new { controller = "Home", action = "Index" }
                    );

                    routes.MapRoute(
                        name: "cmsPage",
                        template: "{slug}",
                        defaults: new { controller = "CmsPage", action = "Index" });

                    routes.MapRoute(
                        name: "blogPost",
                        template: "Post/{slug?}",
                        defaults: new { controller = "Post", action = "Index" });

                    routes.MapRoute(
                        name: "blogCategory",
                        template: "Category/{slug?}",
                        defaults: new { controller = "Category", action = "Index" });

                    routes.MapRoute(
                        name: "MultiLangDefault",
                        template: "{lang:lang}/",
                        defaults: new { controller = "Home", action = "Index" }
                    );

                    routes.MapRoute(
                      name: "ForMultiLangRedirect",
                      template: "{*catchall}",
                      defaults: new { controller = "Home", action = "ResourceNotFound" });
                }
                else
                {
                    routes.MapRoute(
                    name: "login",
                    template: "Login",
                    defaults: new { controller = "Account", action = "Login" });

                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                         defaults: new { controller = "Home", action = "Index" }
                    );

                    routes.MapRoute(
                        name: "cmsPage",
                        template: "{slug}",
                        defaults: new { controller = "CmsPage", action = "Index" });

                    routes.MapRoute(
                        name: "blogPost",
                        template: "Post/{slug?}",
                        defaults: new { controller = "Post", action = "Index" });

                    routes.MapRoute(
                        name: "blogCategory",
                        template: "Category/{slug?}",
                        defaults: new { controller = "Category", action = "Index" });

                    routes.MapRoute(
                        name: "MultiLangDefault",
                        template: "{lang:lang}/",
                        defaults: new { controller = "Home", action = "Index" }
                    );
                }
            });
            return app;
        }
    }
}
