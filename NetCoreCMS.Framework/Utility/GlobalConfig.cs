using NetCoreCMS.Framework.Modules;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace NetCoreCMS.Framework.Utility
{
    public class GlobalConfig
    {
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