/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/
 
using NetCoreCMS.Framework.Core;
using NetCoreCMS.Framework.Modules.Widgets;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
        public Theme()
        {
            Widgets = new List<Widget>();
            Layouts = new List<Layout>();
            MenuLocations = new List<string>();
            Settings = new Hashtable();            
            //Settings.Add("NccVersion", NccInfo.Version);
        }

        public string ThemeId { get; set; }
        public string ThemeName { get; set; }        
        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string DemoUrl { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }        
        public string NccVersion { get; set; }
        public string Category { get; set; }        
        public bool IsActive { get; set; }

        public List<string> MenuLocations { get; set; }
        public List<Layout> Layouts { get; set; }
        public Hashtable Settings { get; set; }

        [JsonIgnore]
        public List<Widget> Widgets { get; set; }

        [JsonIgnore]
        public string Folder { get; set; }

        [JsonIgnore]
        public string ResourceFolder { get; set; }

        [JsonIgnore]
        public string ConfigFilePath { get; set; }

        [JsonIgnore]
        public string AssemblyPath { get; set; }

        public void Save()
        {
            if (File.Exists(ConfigFilePath))
            {
                var themeJson = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(ConfigFilePath, themeJson);
            }
        }
    }
}