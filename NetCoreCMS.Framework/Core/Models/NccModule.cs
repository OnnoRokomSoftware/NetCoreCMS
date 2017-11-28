/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using System.Collections.Generic;
using System;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccModule : BaseModel<long>
    { 
        public NccModule()
        {
            Dependencies = new List<NccModuleDependency>();
        }

        public string ModuleId { get; set; }
        public bool IsCore { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } 
        public string Author { get; set; }
        public string WebSite { get; set; }
        
        public bool AntiForgery { get; set; }
        public string Version { get; set; }
        public string MinNccVersion { get; set; }
        public string MaxNccVersion { get; set; }        
        public string Path { get; set; }
        public string Folder { get; set; }

        public NccModuleStatus ModuleStatus { get; set; }
        public List<NccModuleDependency> Dependencies { get; set; }

        public enum NccModuleStatus
        {        
            New,
            Installed,
            UnInstalled,
            Active,
            Inactive,
            Duplicate,
            Deleted,
            InCompatible
        }
    }     
}
