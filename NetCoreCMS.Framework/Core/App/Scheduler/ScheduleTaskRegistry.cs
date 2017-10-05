using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.App.Scheduler
{
    public class ScheduleTaskRegistry
    {
        private volatile List<IScheduleTask> _tasksInIdleState;
        private volatile List<IScheduleTask> _tasksInWaitingState;
        private volatile List<IScheduleTask> _tasksAreExecutingState;
        private volatile List<IScheduleTask> _tasksAreCompletedState;

        public ScheduleTaskRegistry()
        {
            _tasksInIdleState = new List<IScheduleTask>();
            _tasksInWaitingState = new List<IScheduleTask>();
            _tasksAreExecutingState = new List<IScheduleTask>();
            _tasksAreCompletedState = new List<IScheduleTask>();
        }

        public void LoadTasks()
        {

        }

        public void SaveTasks()
        {

        }

        public void RemoveTasks()
        {

        }

        public void StartUpdatingTaskState()
        {

        }

        public List<IScheduleTask> GetExecutableTasksList()
        {
            return _tasksAreExecutingState;
        }
    }
}
