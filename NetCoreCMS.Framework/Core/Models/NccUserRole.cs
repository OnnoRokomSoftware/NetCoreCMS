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
