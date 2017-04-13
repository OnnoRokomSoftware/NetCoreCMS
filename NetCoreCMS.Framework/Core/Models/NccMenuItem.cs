/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccMenuItem : BaseModel
    {
        public NccMenuItem Parent { get; set; }
        public string Module { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public string Data { get; set; }
        public string Target { get; set; }
        public int Position { get; set; }
        public string MenuIconCls { get; set; }
        public int MenuOrder { get; set; }
        public ActionType MenuActionType { get; set; }
        public MenuItemFor MenuFor { get; set; }

        public enum ActionType
        {
            Url,
            Page,
            Blog,
            Module,
            BlogPost,
            BlogCategory,
            Tag,
        }

        public enum MenuItemFor
        {
            Site,
            Admin
        }

        public class MenuItemTarget
        {
            public static string Blank { get { return "_blank"; } }
            public static string Parent { get { return "_parent"; } }
            public static string Self { get { return "_self"; } }
            public static string Top { get { return "_top"; } }
        }
    }    
}
