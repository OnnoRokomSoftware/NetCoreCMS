using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core
{
    public class CmsInfo
    {
        public static string CmsName { get; } = "NetCoreCMS";
        public static Version Version { get; } = new Version(0,1,1);
        public static string Description { get; } = "A Content Management System developed using ASP.NET Core.";
        public static string Website { get; } = "http://xonaki.com";
        public static string Author { get; } = "Xonaki";
        public static string CoreModuleFolder { get; } = "Core";
        public static string ModuleFolder { get; } = "Modules";
    }
}
