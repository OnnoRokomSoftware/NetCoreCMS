using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;


namespace NetCoreCMS.Core.Modules.Blog
{
    public class BlogModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<NccUser>().ToTable("Ncc_User");
            //modelBuilder.Entity<NccRole>().ToTable("Ncc_Role");
            //modelBuilder.Entity<IdentityUserClaim<long>>(b =>
            //{
            //    b.HasKey(uc => uc.Id);
            //    b.ToTable("Ncc_UserClaim");
            //});
            
        }
    }
}
