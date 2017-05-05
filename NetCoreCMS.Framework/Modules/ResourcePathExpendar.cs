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
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Utility;
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
            
            var activeSiteTheme = GlobalConfig.ActiveTheme.ThemeName;
            var themePath = Path.Combine(env.ContentRootPath, NccInfo.ThemeFolder);
            
            var activeThemePath = Path.Combine(themePath, activeSiteTheme);
            activeThemePath = Path.Combine(activeThemePath, "wwwroot");
            var siteThemeDir = new DirectoryInfo(activeThemePath);
            if (siteThemeDir.Exists)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(siteThemeDir.FullName),
                    RequestPath = new PathString("/Themes/" + activeSiteTheme)
                });
            }
            
        }
    }
}
