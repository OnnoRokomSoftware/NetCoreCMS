using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.App.Scheduler.Basic
{
    public class EmailTaskScheduler :  ScheduleTask, IScheduleTask
    {
        public string ToEmail { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string EmailContent { get; set; }

        public override string Execute()
        {
            return base.Execute();
        }
    }
}
