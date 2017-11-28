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
using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccMenuItem : BaseModel<long>
    { 
        public NccMenuItem()
        {
            SubActions = new List<NccMenuItem>();
            Childrens = new List<NccMenuItem>();
        }
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

        public List<NccMenuItem> SubActions { get; set; }
        public List<NccMenuItem> Childrens { get; set; }


        public enum ActionType
        {
            Url,
            Page,
            BlogPost,
            BlogCategory,
            Module,
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
