using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Analytics.Models
{
    public class AnalyticsModel : BaseModel, IBaseModel<long>
    {
        public AnalyticsModel()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string Key { get; set; }
        public List<AnalyticsLogModel> AnalyticsLogList { get; set; }
    }
}