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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreCMS.Framework.Core.Models
{
    /// <summary>
    /// Assigned user's permissions
    /// </summary>
    public class NccUserPermission
    {
        public long UserId { get; set; }
        public long PermissionId { get; set; }

        public NccUser User { get; set; }
        public NccPermission Permission { get; set; }
    }
}
