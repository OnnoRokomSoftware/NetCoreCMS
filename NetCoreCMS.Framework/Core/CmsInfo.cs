using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core
{
    public class CmsInfo
    {
        public static string CmsName { get; set; } = "NetCoreCMS";
        public static Version Version { get; set; } = new Version(0,1,1);
        public static string Description { get; set; } = "A Content Management System developed using ASP.NET Core.";
        public static string Website { get; set; } = "http://xonaki.com";
        public static string Author { get; set; } = "Xonaki";
    }
}
