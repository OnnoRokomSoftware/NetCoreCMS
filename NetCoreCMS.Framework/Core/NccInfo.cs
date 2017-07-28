using System;

namespace NetCoreCMS.Framework.Core
{
    public class NccInfo
    {
        public static string Name { get; } = "NetCoreCMS";
        public static string Slogan { get; } = "An ASP.Net Core CMS as Site Engine";
        public static Version Version { get; } = new Version(0,3,1);
        public static string Description { get; } = "A Content Management System developed using ASP.NET Core.";
        public static string Website { get; } = "http://DotNetCoreCMS.com";
        public static string Email { get; } = "netcorecms@gmail.com";
        public static string Author { get; } = "DotNetCoreCMS.com";
        public static string CoreModuleFolder { get; } = "Core";
        public static string ModuleFolder { get; } = "Modules";
        public static string ThemeFolder { get; } = "Themes";
        public static string LogFolder { get; } = "Logs";

    }
}
