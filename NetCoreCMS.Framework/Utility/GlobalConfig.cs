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
using Microsoft.Extensions.Configuration;

namespace NetCoreCMS.Framework.Utility
{
    public class GlobalConfig
    {
        public static bool IsRestartRequired { get; set; }
        public static List<INccModule> Modules { get; set; } = new List<INccModule>();
        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }
        public static IServiceCollection Services { get; set; }
        public static IApplicationBuilder App { get; set; }
        public string SiteBaseUrl { get; set; }
        public string StartupController { get; set; }
        public string StartupAction { get; set; }
    }
}