using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.LinkShare.Models
{
    public class LsLink : IBaseModel<long>
    {
        public LsLink()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
            Order = 1;
            PublishDate = DateTime.Now;
            ExpireDate = DateTime.Now.AddDays(7);
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

        public string Link { get; set; }
        public string ImagePath { get; set; }
        public bool HasDateRange { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate{ get; set; }
        public int Order { get; set; }
        public List<LsLinkCategory> Categories { get; set; }
    }
}