/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;

namespace NetCoreCMS.Framework.Themes
{
    public class SiteMenu : Attribute
    {
        public string Name { get; set; }
        public string IconCls { get; set; }
        public int Order { get; set; }
    }

    public class SiteMenuItem : Attribute
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string QueryString { get; set; }
        public int Order { get; set; }
        public string IconCls { get; set; }
    }
}
