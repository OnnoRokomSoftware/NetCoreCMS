/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

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
