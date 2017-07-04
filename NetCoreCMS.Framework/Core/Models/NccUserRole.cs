using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccUserRole : IdentityUserRole<long>
    {
        public override long UserId { get; set; }

        public NccUser User { get; set; }

        public override long RoleId { get; set; }

        public NccRole Role { get; set; }
    }
}
