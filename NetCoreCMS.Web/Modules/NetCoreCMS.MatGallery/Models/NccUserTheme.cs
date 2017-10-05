using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.MatGallery.Models
{
    public class NccUserTheme : BaseModel, IBaseModel<long>
    {
        public NccUserTheme()
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

        public string ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string Category { get; set; }

        public bool IsPrivate { get; set; }
    }

    public class NccUserThemeLog : BaseModel, IBaseModel<long>
    {
        public NccUserThemeLog()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }

        public string ThemeId { get; set; }
        public string ThemeName { get; set; }
        public string Description { get; set; }

        public string Author { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }

        public string Category { get; set; }

        public bool IsPrivate { get; set; }

        public NccUserTheme nccUserTheme { get; set; }
    }
}