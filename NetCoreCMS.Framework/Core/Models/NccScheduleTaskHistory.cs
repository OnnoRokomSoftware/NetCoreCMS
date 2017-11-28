/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models
{
    [Serializable]
    public class NccScheduleTaskHistory : BaseModel<long>
    {
        public string TaskId { get; set; }
        public string TaskType { get; set; }
        public string TaskOf { get; set; }
        public string TaskCreator { get; set; }
        public string Data { get; set; }
    }
}
