/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;

namespace NetCoreCMS.Framework.Core
{
    public class NccInfo
    {
        public static string Name { get; } = "NetCoreCMS";
        public static string Slogan { get; } = "An ASP.Net Core CMS as Site Engine";
        public static Version Version { get; } = new Version(0,4,5);
        public static string Description { get; } = "A Content Management System developed using ASP.NET Core.";
        public static string Website { get; } = "http://DotNetCoreCMS.org";
        public static string Email { get; } = "info@onnorokomsoftware.com";
        public static string Author { get; } = "DotNetCoreCMS.org";
        public static string CoreModuleFolder { get; } = "Core";
        public static string ModuleFolder { get; } = "Modules";
        public static string ThemeFolder { get; } = "Themes";
        public static string LogFolder { get; } = "__Logs";
        public static string DevelopedBy { get; } = "OnnoRokom Software Ltd.";

    }
}