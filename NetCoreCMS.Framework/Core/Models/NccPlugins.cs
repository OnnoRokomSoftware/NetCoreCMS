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
    public class NccPlugins : BaseModel
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
        public NccPluginsStatus PluginsStatus { get; set; }
        public List<NccWidget> Widgets { get; set; }

        public enum NccPluginsStatus
        {
            Listed,
            Installed,
            Active,
            Inactive,
            Uninstalled
        }
    }
}
