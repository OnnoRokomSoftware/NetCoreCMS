using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models
{
    public class Module : BaseModel
    {
        public bool AntiForgery { get; set; }
        public string Author { get; set; }
        public string Website { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string Description { get; set; }
        public string Dependencies { get; set; }
        public string Category { get; set; }
        public string SortName { get; set; }
        public string Path { get; set; }
        public ModuleStatus ModuleStatus { get; set; }
    }

    public enum ModuleStatus
    {
        Listed,
        Installed,
        Active,
        Inactive,
        Uninstalled
    }
}
