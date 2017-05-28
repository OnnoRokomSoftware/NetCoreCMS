using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Themes
{
    public class Layout
    {
        public string Name { get; set; }
        public string PreviewImage { get; set; }
        public List<string> LayoutZones { get; set; }
    }

    public class Theme
    {
        public string ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string Type { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string Category { get; set; }
        public string PreviewImage { get; set; }
        public bool IsActive { get; set; }
        public List<Layout> Layouts { get; set; }
    }
}
