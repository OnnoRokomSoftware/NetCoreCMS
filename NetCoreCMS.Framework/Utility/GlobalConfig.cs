using NetCoreCMS.Framework.Modules;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Utility
{
    public class GlobalConfig
    {
        public static List<INccModule> Modules { get; set; }
        public static string WebRootPath { get; set; }
        public static string ContentRootPath { get; set; }
        public string SiteBaseUrl { get; set; }
        public string StartupController { get; set; }
        public string StartupAction { get; set; }
    }
}