using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Themes
{
    public class AdminMenu : Attribute
    {
        public string Name { get; set; }
        public string IconCls { get; set; }
        public int Order { get; set; }
    }

    public class AdminMenuItem : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public int Order { get; set; }
        public string IconCls { get; set; }
    }
}
