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
    public class NccTheme : BaseModel
    {   
        public string ThemeName { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string Category { get; set; }
        public ThemeType Type { get; set; }
        public List<NccWidgetSection> WidgetSections { get; set; }
        public List<NccThemeLayout> ThemeLayouts { get; set; }

        public enum ThemeType
        {
            WebSite,
            Admin
        }
    }
}
