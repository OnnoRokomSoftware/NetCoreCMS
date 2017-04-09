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
        public MenuType MenuType { get; set; }
        public string MenuIconCls { get; set; }
        public List<NccMenuItem> MenuItems { get; set; }
        public MenuFor MenuFor { get; set; }
    }

    public enum MenuType
    {
        Top,
        Main,
        Left,
        Right,
        Footer
    }
    public enum MenuFor
    {
        Site,
        Admin,
        Settings
    }
}
