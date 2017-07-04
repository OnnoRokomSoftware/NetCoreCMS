using System;

namespace NetCoreCMS.Framework.Core
{
    public class NccInfo
    {
        public static string Name { get; } = "NetCoreCMS";
        public static string Slogan { get; } = "An ASP.Net Core CMS as Site Engine";
        public static Version Version { get; } = new Version(0,2,2);
        public static string Description { get; } = "A Content Management System developed using ASP.NET Core.";
        public static string Website { get; } = "http://NetCoreCMS-Bangladesh.com";
        public static string Email { get; } = "netcorecms@gmail.com";
        public static string Author { get; } = "NetCoreCMS-Bangladesh.com";
        public static string CoreModuleFolder { get; } = "Core";
        public static string ModuleFolder { get; } = "Modules";
        public static string ThemeFolder { get; } = "Themes";
    }
}
