using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.MatGallery.Models
{
    public class NccUserModule : BaseModel, IBaseModel<long>
    {
        public NccUserModule()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
            IsPrivate = true;
        }


        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }

        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string Category { get; set; }

        public bool IsPrivate { get; set; }
    }

    public class NccUserModuleLog : BaseModel, IBaseModel<long>
    {
        public NccUserModuleLog()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }

        public string ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string Category { get; set; }

        public bool IsPrivate { get; set; }

        public NccUserModule nccUserModule { get; set; }
    }
}