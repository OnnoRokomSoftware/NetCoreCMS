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
    public class NccTheme : IBaseModel<long>
    {
        public NccTheme()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.New;
            VersionNumber = 1;
        }

        [Key]
        public long Id { get; set; }
        public string ThemeId { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }
        public string ThemeName { get; set; }
        public string Version { get; set; }
        public string NetCoreCMSVersion { get; set; }
        public ThemeType Type { get; set; }
        public string PreviewImage { get; set; }      
       
        public enum ThemeType
        {
            WebSite,
            Admin
        }

        public enum ThemeStatus
        {
            Installed,
            UnInstalled,
            Active,
            Inactive
        }
    }
}
