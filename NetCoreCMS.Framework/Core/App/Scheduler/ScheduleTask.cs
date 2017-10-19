/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.App.Scheduler
{
    public class ScheduleTask : IScheduleTask
    {
        public string TaskId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public string TaskOf { get; set; }
        public string TaskCreator { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TaskType Type { get; set; } 
        public TaskTigger Tigger { get; set; }
        public TaskState State { get; set; }
        public TaskExecutationStrategy Strategy { get; set; }
        public TaskStatus Status { get; set; }
        

        public int Interval { get; set; }

        public virtual string Execute()
        {
            return "kam sesh";
        }
    }

    public enum TaskType
    {
       Once,
       Minutely,
       Hourly,
       Daily,
       Weekly,
       Monthly,
       Yearly
    }
     
    public enum TaskTigger
    {
        BeforeSiteStart,
        AfterSiteStart
    }

    public enum TaskStatus
    {
        Active,
        Inactive,
        Suspended
    }

    public enum TaskState
    {
        Idle,
        Waiting,
        Executation,
        Completed
    }

    public enum TaskExecutationStrategy
    {
        WaitForExecution,
        QueueForExecution,
        ExecuteParallely,
        SuspendExecution
    }
}
