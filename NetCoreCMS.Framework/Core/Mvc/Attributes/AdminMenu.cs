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

namespace NetCoreCMS.Framework.Core.Mvc.Attributes
{
    public class AdminMenu : Attribute, IMenu
    {
        public string Name { get; set; }
        public string IconCls { get; set; }
        public int Order { get; set; }
        public string Url { get; set; }
        public bool IsVisible { get; set; } = true;
    }

    public class AdminMenuItem : Attribute, IMenuItem
    {
        public AdminMenuItem()
        {
            SubActions = new string[] { };
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string QueryString { get; set; }
        public int Order { get; set; }
        public string IconCls { get; set; }
        public string[] SubActions { get; set; }
        public bool HasAllowAnonymous { get; set; }
        public bool IsVisible { get; set; } = true;
    }
}
