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
    public class NccThemeLayout : BaseModel
    {   
        public NccTheme Theme { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string LayoutImage { get; set; }
        public string FileName { get; set; }
        public List<NccWidgetSection> WidgetSections { get; set; }
    }
}
