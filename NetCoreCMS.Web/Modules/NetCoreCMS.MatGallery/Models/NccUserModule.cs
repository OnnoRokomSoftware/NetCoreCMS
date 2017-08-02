using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.MatGallery.Models
{
    public class NccUserModule : IBaseModel<long>
    {
        public NccUserModule()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        [Key]
        public long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }


        public string ModuleVersion { get; set; }
        public string NetCoreCMSVersion { get; set; }

        public string ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string CategoryList { get; set; }
    }

    public class NccUserModuleLog : IBaseModel<long>
    {
        public NccUserModuleLog()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Archived;
            VersionNumber = 1;
        }

        [Key]
        public long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }


        public string ModuleVersion { get; set; }
        public string NetCoreCMSVersion { get; set; }

        public string ModuleId { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string CategoryList { get; set; }

        public NccUserModule nccUserModule { get; set; }
    }
}