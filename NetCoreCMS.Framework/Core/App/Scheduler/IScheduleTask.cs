using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.App.Scheduler
{
    public interface IScheduleTask
    {
        string Name { get; set; }
        string Description { get; set; }
        int Priority { get; set; }
        string TaskOf { get; set; }
        string TaskCreator { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        TaskType Type { get; set; }
        TaskTigger Tigger { get; set; }
        TaskState State { get; set; }
        TaskExecutationStrategy Strategy { get; set; }
        TaskStatus Status { get; set; }
        
        string Execute();
    }
}
