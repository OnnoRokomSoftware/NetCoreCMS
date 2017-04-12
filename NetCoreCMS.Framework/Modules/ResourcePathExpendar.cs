/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NetCoreCMS.Framework.Modules
{
    public class ResourcePathExpendar
    {
        public static void RegisterStaticFiles(IHostingEnvironment env, IApplicationBuilder app, IList<IModule> modules)
        {
            foreach (var module in modules)
            {
                var moduleDir = new DirectoryInfo(Path.Combine(module.Path, "wwwroot"));
                if (moduleDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(moduleDir.FullName),
                        RequestPath = new PathString("/" + module.ModuleName)
                    });
                }
            }

            var activeAdminTheme = "Default";
            var activeSiteTheme = "Default";
            var themePath = Path.Combine(env.ContentRootPath, "Themes");

            var siteThemePath = Path.Combine(themePath, "Site");
            siteThemePath = Path.Combine(siteThemePath, activeSiteTheme);
            siteThemePath = Path.Combine(siteThemePath, "wwwroot");
            var siteThemeDir = new DirectoryInfo(siteThemePath);
            if (siteThemeDir.Exists)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(siteThemeDir.FullName),
                    RequestPath = new PathString("/Theme/Site/" + activeSiteTheme)
                });
            }
            
            var adminThemePath = Path.Combine(themePath, "Admin");
            adminThemePath = Path.Combine(adminThemePath, activeAdminTheme);
            adminThemePath = Path.Combine(adminThemePath, "wwwroot");

            var adminThemeDir = new DirectoryInfo(adminThemePath);
            if (adminThemeDir.Exists)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(adminThemeDir.FullName),
                    RequestPath = new PathString("/Theme/Admin/" + activeAdminTheme)
                });
            }
            
        }
    }
}
