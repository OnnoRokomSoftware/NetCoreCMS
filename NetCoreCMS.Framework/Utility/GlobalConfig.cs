using NetCoreCMS.Framework.Modules;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Themes;
using System;
using NetCoreCMS.Framework.Modules.Widgets;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace NetCoreCMS.Framework.Utility
{
    public class GlobalConfig
    {
        public GlobalConfig()
        {
            
        }
        public static NccWebSite WebSite { get; set; }
        public static bool IsRestartRequired { get; set; }
        public static List<IModule> Modules { get; set; } = new List<IModule>();
        public static List<IWidget> Widgets{ get; set; } = new List<IWidget>();
        public static List<NccWebSiteWidget> WebSiteWidgets { get; set; } = new List<NccWebSiteWidget>();
        public static List<Theme> Themes { get; set; } = new List<Theme>();
        public static List<NccMenu> Menus { get; set; } = new List<NccMenu>();
        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }
        public static IServiceCollection Services { get; set; }
        public static IApplicationBuilder App { get; set; }
        public static Theme ActiveTheme { get; set; }
        
        public string SiteBaseUrl { get; set; }
        public string StartupController { get; set; }

        public static List<IModule> GetActiveModules()
        {
            var query = from m in Modules where m.ModuleStatus == (int) NccModule.NccModuleStatus.Active select m;
            return query.ToList();
        }

        public string StartupAction { get; set; }

        public static void ListWidgets()
        {
            foreach (var item in Modules)
            {
                Widgets.AddRange(item.Widgets);
            }
        }
    }
}