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

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccStartup : BaseModel<long>
    {  
        public long UserId { get; set; }
        //public NccUser User { get; set; }
        public long RoleId { get; set; }
        
        public StartupType StartupType { get; set; }
        public string StartupUrl { get; set; }
        public StartupFor StartupFor { get; set; }

        public NccRole Role { get; set; }
    }

    public enum StartupFor
    {
        Website,
        Admin,
        Role,
        User
    }

    public enum StartupType
    {
        Url,
        Page,
        Category,
        Post,
        Module,
        Tag
    }
}
