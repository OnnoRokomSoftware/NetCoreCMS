/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccModule : IBaseModel<long>
    {
        public NccModule()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.New;
            VersionNumber = 1;
        }

        [Key]
        public long Id { get; set; }
        public string ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } 
        public string Author { get; set; }
        public string WebSite { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }
        public bool AntiForgery { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public string Dependencies { get; set; }
        public string Path { get; set; }

        public NccModuleStatus ModuleStatus { get; set; }

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
