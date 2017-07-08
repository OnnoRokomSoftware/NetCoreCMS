using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Utility
{
    public static class Constants
    {
        public static string SiteLayoutName { get; } = "_SiteLayout";
        public static string AdminLayoutName { get; } = "_AdminLayout";
        public static string ModuleConfigFileName { get; } = "Module.json";
        public static string ThemeConfigFileName { get; } = "Theme.json";

        public static string SiteUrl { get;} = "CmsHome";
        public static string AdminUrl { get;} = "Admin";
        public static string NotFoundUrl { get; } = "/CmsHome/ResourceNotFound";

    }
}
