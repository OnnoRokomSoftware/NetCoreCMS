/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using System.Collections.Generic;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    /// <summary>
    /// From permission management admin will assign permission to the user. That 
    /// </summary>
    public class NccUserAuthPolicyRequirement : BaseModel<long>
    {
        public NccUser User { get; set; }
        public string ModuleId { get; set; }
        public string PolicyName { get; set; }
        public string Requirement { get; set; }
        public string RequirementValue { get; set; }

        public NccUserAuthPolicyRequirement()
        {

        }
    }
}
