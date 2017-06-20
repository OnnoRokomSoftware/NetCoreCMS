/*
 * Author: TecRT
 * Website: http://tecrt.com
 * Copyright (c) tecrt.com
 * License: BSD (3 Clause)
*/
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;


namespace NetCoreCMS.Core.Modules.Cms.Models
{
    public class CmsModelBuilder : INccModuleBuilder
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
            //modelBuilder.Entity<IdentityRoleClaim<long>>(b =>
            //{
            //    b.HasKey(rc => rc.Id);
            //    b.ToTable("Ncc_RoleClaim");
            //});
            //modelBuilder.Entity<NccUserRole>(b =>
            //{
            //    b.HasKey(ur => new { ur.UserId, ur.RoleId });
            //    b.HasOne(ur => ur.Role).WithMany(r => r.Users).HasForeignKey(r => r.RoleId);
            //    b.HasOne(ur => ur.User).WithMany(u => u.Roles).HasForeignKey(u => u.UserId);
            //    b.ToTable("Ncc_UserRole");
            //});

            //modelBuilder.Entity<IdentityUserLogin<long>>(b =>
            //{
            //    b.ToTable("Ncc_UserLogin");
            //});

            //modelBuilder.Entity<IdentityUserToken<long>>(b =>
            //{
            //    b.ToTable("Ncc_UserToken");
            //});

            //modelBuilder.Entity<BaseModel>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.CreateBy);
            //    e.Property(x => x.CreationDate);
            //    e.Property(x => x.ModificationDate);
            //    e.Property(x => x.ModifyBy);
            //    e.Property(x => x.Name);
            //    e.Property(x => x.Status);
            //    e.Property(x => x.VersionNumber);
            //});
            
        }
    }
}
