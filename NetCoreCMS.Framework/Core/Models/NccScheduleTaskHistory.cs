/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

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
