using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccWebSiteWidget : IBaseModel<long>
    {
        public NccWebSiteWidget()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            CreateBy = ModifyBy = BaseModel.GetCurrentUserId();
            Status = EntityStatus.New;
            VersionNumber = 1;
            Status = 1;
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
        public NccWebSite WebSite { get; set; }
        public string ThemeId { get; set; }
        public string ModuleId { get; set; }
        public string LayoutName { get; set; }
        public string Zone { get; set; }
        public string WidgetId { get; set; }
        public int WidgetOrder { get; set; }
        public string WidgetConfigJson { get; set; }
        public string WidgetData { get; set; }
    }

    public enum WebSiteWidgetStatus
    {
        Active,
        Inactive,
        Installed,
        Uninstalled
    }
}
