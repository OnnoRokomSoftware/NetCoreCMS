/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.AdvancedPermission.Models
{
    /// <summary>
    /// From permission management admin will assign permission to the user. That 
    /// </summary>
    public class NccUserPermission: BaseModel<long>
    {
        public NccUser User { get; set; }
        public string ModuleId { get; set; }        
        public string Controller { get; set; }
        public string Action { get; set; }

        public NccUserPermission()
        {

        }
    }
}
