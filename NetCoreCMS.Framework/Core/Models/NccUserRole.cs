/*************************************************************  
 *          Project: NetCoreCMS                              *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *           Mobile: +88 017 08 166 003                      *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Identity;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccUserRole : IdentityUserRole<long>
    {
        public override long UserId { get; set; }
        public override long RoleId { get; set; }

        public NccUser User { get; set; }
        public NccRole Role { get; set; }
    }
}
