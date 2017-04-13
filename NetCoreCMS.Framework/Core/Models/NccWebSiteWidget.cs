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
    public class NccWebSiteWidget : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public NccWebSite WebSite { get; set; }
        public NccTheme Theme { get; set; }
        public NccThemeLayout Layout { get; set; }
        public NccWidgetSection Section { get; set; }
        public NccWidget Widget { get; set; }
        public int WidgetOrder { get; set; }
        public string WidgetConfigJson { get; set; }
    }
}
