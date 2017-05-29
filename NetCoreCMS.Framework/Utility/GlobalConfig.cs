/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Modules;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Themes;
using System;
using NetCoreCMS.Framework.Modules.Widgets;

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
        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }
        public static IServiceCollection Services { get; set; }
        public static IApplicationBuilder App { get; set; }
        public static Theme ActiveTheme { get; set; }
        public string SiteBaseUrl { get; set; }
        public string StartupController { get; set; }
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