using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Analytics.Models
{
    public class AnalyticsLogModel : BaseModel, IBaseModel<long>
    {
        public AnalyticsLogModel()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.Active;
            VersionNumber = 1;
        }

        public string VisitUrl { get; set; }
        public string ReferrerUrl { get; set; }
        public string Browser { get; set; }
        public string BrowserVersion { get; set; }
        public string Platform { get; set; }
        public bool IsMobile { get; set; }
        public string MobileDeviceModel { get; set; }
        public string IpAddress { get; set; }
        public string BrowserAgent { get; set; }

        public AnalyticsModel AnalyticsModel { get; set; }
    }
}