using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccScheduleTaskHistory : BaseModel
    {
        public string TaskId { get; set; }
        public string TaskType { get; set; }
        public string TaskOf { get; set; }
        public string TaskCreator { get; set; }
        public string Data { get; set; }
    }
}
