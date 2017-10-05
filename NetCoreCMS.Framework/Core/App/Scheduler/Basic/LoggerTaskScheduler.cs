using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.App.Scheduler.Basic
{
    public class LoggerTaskScheduler : ScheduleTask, IScheduleTask
    {
        public string LogText { get; set; }
        public override string Execute()
        {
            return base.Execute();
        }
    }
}
