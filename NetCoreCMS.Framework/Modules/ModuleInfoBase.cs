using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Modules
{
    public class ModuleInfoBase
    {
        public string ModuleName { get; set; }        
        public bool AntiForgery { get; set; }
        public string Author { get; set; }
        public string Website { get; set; } 
        public static Version Version { get; set; }
        public Version NetCoreCMSVersion { get; set; }
        public string Description { get; set; }
        public List<string> Dependencies { get; set; }
        public List<string> Category { get; set; }
    }
}
