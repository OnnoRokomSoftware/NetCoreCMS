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

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccStartup : BaseModel, IBaseModel<long>
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
