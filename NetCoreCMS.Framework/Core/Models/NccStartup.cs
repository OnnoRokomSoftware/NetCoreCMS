using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccStartup : IBaseModel<long>
    {
        public NccStartup()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.New;
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

        public long UserId { get; set; }
        public NccUser User { get; set; }
        public long RoleId { get; set; }
        public NccRole Role { get; set; }
        public StartupType StartupType { get; set; }
        public string StartupUrl { get; set; }
        public StartupFor StartupFor { get; set; }
    }

    public enum StartupFor
    {
        Website,
        Admin,
        Role,
        User
    }

    public enum StartupType
    {
        Default,
        Page,
        PostCategory,
        Post,
        Module,
        Tag
    }
}
