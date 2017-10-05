using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.News.Models
{
    public class NeNews : BaseModel, IBaseModel<long>
    {
        public NeNews()
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

        public string Content { get; set; }
        public string Excerpt { get; set; }
        public bool HasDateRange { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime ExpireDate{ get; set; }
        public int Order { get; set; }
        public List<NeNewsCategory> CategoryList { get; set; }
    }
}