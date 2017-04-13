/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccMenu : BaseModel
    {
        public MenuPosition Position { get; set; }
        public string MenuIconCls { get; set; }
        public List<NccMenuItem> MenuItems { get; set; }
        public NccMenuFor MenuFor { get; set; }
        public int MenuOrder { get; set; }

        public enum MenuPosition
        {
            Top,
            Main,
            Left,
            Right,
            Footer
        }
        public enum NccMenuFor
        {
            Site,
            Admin
        }
    }   
}
