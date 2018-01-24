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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Themes;
using NetCoreCMS.Framework.Utility;
using System.Collections.Generic;
using System.IO;
using System;
using NetCoreCMS.Framework.Modules;

namespace NetCoreCMS.Framework.Resources
{
    public class ResourcePathExpendar
    {
        public static void RegisterStaticFiles(IHostingEnvironment env, IApplicationBuilder app, IList<IModule> modules, List<Theme> themes)
        {
            RegisterModulesResourcePath(env,app,modules);
            RegisterActiveThemeResourcePath(env,app,themes);
            RegisterChildThemeResourcePath(env, app, themes);
            RegisterThemesPreviewResourcePath(env,app,themes);
        }

        private static void RegisterThemesPreviewResourcePath(IHostingEnvironment env, IApplicationBuilder app, List<Theme> themes)
        {
            foreach (var item in themes)
            {
                var themeFolder = Path.Combine(env.ContentRootPath, NccInfo.ThemeFolder);

                var themePath = Path.Combine(themeFolder, item.ThemeId);
                themePath = Path.Combine(themePath, "Meta");
                var siteThemeDir = new DirectoryInfo(themePath);
                if (siteThemeDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(siteThemeDir.FullName),
                        RequestPath = new PathString("/Themes/" + item.ThemeId + "/Meta")
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

                var moduleMetaDir = new DirectoryInfo(Path.Combine(module.Path, "Meta"));
                if (moduleMetaDir.Exists)
                {
                    app.UseStaticFiles(new StaticFileOptions()
                    {
                        FileProvider = new PhysicalFileProvider(moduleMetaDir.FullName),
                        RequestPath = new PathString("/" + GetModuleFolderName(module.Path)+"/Meta")
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
            var activeSiteTheme = ThemeHelper.ActiveTheme.ThemeId;
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

        private static void RegisterChildThemeResourcePath(IHostingEnvironment env, IApplicationBuilder app, List<Theme> themes)
        {
            var activeSiteTheme = ThemeHelper.ActiveTheme.ThemeId;
            var themePath = Path.Combine(env.ContentRootPath, NccInfo.ThemeFolder);

            var childThemePath = Path.Combine(themePath, "ChildThemes", activeSiteTheme, "wwwroot");            
            var siteThemeDir = new DirectoryInfo(childThemePath);
            if (siteThemeDir.Exists)
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(siteThemeDir.FullName),
                    RequestPath = new PathString("/Themes/ChildThemes/" + activeSiteTheme)
                });
            }
        }
    }
}
