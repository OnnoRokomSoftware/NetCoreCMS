using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;
using System.IO;
using System;

namespace NetCoreCMS.Framework.Modules
{
    public class ResourcePathExpendar
    {
        public static void RegisterStaticFiles(IHostingEnvironment env, IApplicationBuilder app, IList<IModule> modules, List<Theme> themes)
        {
            RegisterModulesResourcePath(env,app,modules);
            RegisterActiveThemeResourcePath(env,app,themes);
            RegisterThemesPreviewResourcePath(env,app,themes);
        }

        private static void RegisterThemesPreviewResourcePath(IHostingEnvironment env, IApplicationBuilder app, List<Theme> themes)
        {
            foreach (var item in themes)
            {
                var themePath = Path.Combine(env.ContentRootPath, NccInfo.ThemeFolder);

                var activeThemePath = Path.Combine(themePath, item.ThemeName);
                activeThemePath = Path.Combine(activeThemePath, "Preview");
                var siteThemeDir = new DirectoryInfo(activeThemePath);
                if (siteThemeDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(siteThemeDir.FullName),
                        RequestPath = new PathString("/Themes/" + item.ThemeName + "/Preview")
                    });
                }
            }
        }

        private static void RegisterModulesResourcePath(IHostingEnvironment env, IApplicationBuilder app, IList<IModule> modules)
        {
            foreach (var module in modules)
            {
                var moduleDir = new DirectoryInfo(Path.Combine(module.Path, "wwwroot"));
                if (moduleDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(moduleDir.FullName),
                        RequestPath = new PathString("/" + GetModuleFolderName(module.Path))
                    });
                }
            }
        }

        private static string GetModuleFolderName(string path)
        {
            var dir = new DirectoryInfo(path);
            return dir.Name;
        }

        private static void RegisterActiveThemeResourcePath(IHostingEnvironment env, IApplicationBuilder app, List<Theme> themes)
        {
            var activeSiteTheme = ThemeHelper.ActiveTheme.ThemeName;
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
