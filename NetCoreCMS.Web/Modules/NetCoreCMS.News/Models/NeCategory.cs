using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.News.Models
{
    public class NeCategory : BaseModel, IBaseModel<long>
    {
        public NeCategory()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
            NewsList = new List<NeNewsCategory>();
        } 

        public List<NeNewsCategory> NewsList { get; set; }
    }
}