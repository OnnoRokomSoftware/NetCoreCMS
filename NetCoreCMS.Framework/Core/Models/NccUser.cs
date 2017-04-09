/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Mvc.Models;
using System;

namespace NetCoreCMS.Framework.Core.Models
{
    public class NccUser : IdentityUser<long, IdentityUserClaim<long>, NccUserRole, IdentityUserLogin<long>>, IBaseModel<long>
    {
        public NccUser()
        {
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        public string Slug { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public long CreateBy { get; set; }
        public long ModifyBy { get; set; }
        public int Status { get; set; }
    }
}
