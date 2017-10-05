using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreCMS.Modules.Analytics.Models
{
    public class ViewAnalyticsModel
    {
        public long Id { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }

        public string Key { get; set; }
        public int TotalLog { get; set; }
        public int TotalLogToday { get; set; }
        public int TotalLogLast24 { get; set; }
    }
}