/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;
using NetCoreCMS.Framework.Core.Mvc.Models;


namespace NetCoreCMS.Framework.Auth
{
    public class CoreModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccUser>().ToTable("Ncc_User");
            modelBuilder.Entity<NccRole>().ToTable("Ncc_Role");
            modelBuilder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("Ncc_UserClaim");
            });
            modelBuilder.Entity<IdentityRoleClaim<long>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("Ncc_RoleClaim");
            });
            modelBuilder.Entity<NccUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.HasOne(ur => ur.Role).WithMany(r => r.Users).HasForeignKey(r => r.RoleId);
                b.HasOne(ur => ur.User).WithMany(u => u.Roles).HasForeignKey(u => u.UserId);
                b.ToTable("Ncc_UserRole");
            });

            modelBuilder.Entity<IdentityUserLogin<long>>(b =>
            {
                b.ToTable("Ncc_UserLogin");
            });

            modelBuilder.Entity<IdentityUserToken<long>>(b =>
            {
                b.ToTable("Ncc_UserToken");
            });

            modelBuilder.Entity<NccWebSite>().ToTable("Ncc_WebSite");
            modelBuilder.Entity<NccModule>().ToTable("Ncc_Module");
            modelBuilder.Entity<NccTheme>().ToTable("Ncc_Theme");
            modelBuilder.Entity<NccSettings>().ToTable("Ncc_Settings");

            modelBuilder.Entity<NccMenu>(b => {
                b.HasMany(ur => ur.MenuItems);
                b.ToTable("Ncc_NccMenu");
            });

            modelBuilder.Entity<NccMenuItem>(b=> {
                b.HasOne(ur => ur.Parent);
                b.ToTable("Ncc_MenuItem");
            });

        }
    }
}
