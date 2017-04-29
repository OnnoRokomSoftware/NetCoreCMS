/*
 * Author: Xonaki
 * Website: http://xonaki.com
 * Copyright (c) xonaki.com
 * License: BSD (3 Clause)
*/
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Mvc.Models;

namespace NetCoreCMS.Framework.Core.Models
{
    public class CoreModelBuilder : INccModuleBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccMenu>(b => {
                b.ToTable("Ncc_NccMenu");
                b.HasMany(ur => ur.MenuItems);
            });
            modelBuilder.Entity<NccMenuItem>(b => {
                b.ToTable("Ncc_MenuItem");
                b.HasOne( m => m.Parent );
                b.HasMany( m => m.SubActions );
                b.HasMany(m => m.Childrens);
            });
            modelBuilder.Entity<NccModule>(b => {
                b.ToTable("Ncc_Module");
                b.HasMany(m => m.Widgets);
            });
            modelBuilder.Entity<NccPage>(b => {
                b.ToTable("Ncc_Page");
                b.HasOne(p => p.Parent);
            });
            modelBuilder.Entity<NccPlugins>(b => {
                b.ToTable("Ncc_Plugins");
                b.HasMany(p => p.Widgets);
            });
            modelBuilder.Entity<NccPost>(b => {
                b.ToTable("Ncc_NccPost");
                b.HasOne(p => p.Parent);
                b.HasOne(p => p.Author);
                b.HasMany(p => p.Categories);
                b.HasMany(p => p.PostComments);
                b.HasMany(p => p.Tags);
            });
            modelBuilder.Entity<NccPostCategory>(b => {
                b.ToTable("Ncc_PostCategory");
                b.HasOne(p => p.Parent);
                b.HasMany(p => p.Posts);
            });
            modelBuilder.Entity<NccPostComment>(b => {
                b.ToTable("Ncc_PostComment");
                b.HasOne(p => p.Post);
                b.HasOne(p => p.Author);
            });
            modelBuilder.Entity<NccRole>().ToTable("Ncc_Role");
            modelBuilder.Entity<NccSettings>().ToTable("Ncc_Settings");
            modelBuilder.Entity<NccTag>().ToTable("Ncc_Tag");
            modelBuilder.Entity<NccTheme>(b => {
                b.ToTable("Ncc_Theme");
                b.HasMany(t => t.ThemeLayouts);
            });
            modelBuilder.Entity<NccThemeLayout>(b => {
                b.ToTable("Ncc_ThemeLayout");
                b.HasOne(t => t.Theme);
                b.HasMany(t => t.WidgetSections);
            });            
            modelBuilder.Entity<NccUser>().ToTable("Ncc_User");
            modelBuilder.Entity<NccUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.HasOne(ur => ur.Role).WithMany(r => r.Users).HasForeignKey(r => r.RoleId);
                b.HasOne(ur => ur.User).WithMany(u => u.Roles).HasForeignKey(u => u.UserId);
                b.ToTable("Ncc_UserRole");
            });
            modelBuilder.Entity<NccWebSite>().ToTable("Ncc_WebSite");
            modelBuilder.Entity<NccWebSiteWidget>(b => {
                b.ToTable("Ncc_WebSiteWidget");
                b.HasOne(w => w.WebSite);
                b.HasOne(w => w.Theme);
                b.HasOne(w => w.Layout);
                b.HasOne(w => w.Section);
                b.HasOne(w => w.Widget);
            });
            modelBuilder.Entity<NccWidget>().ToTable("Ncc_Widget");
            modelBuilder.Entity<NccWidgetSection>(b => {
                b.ToTable("Ncc_WidgetSections");

            });
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
            modelBuilder.Entity<IdentityUserLogin<long>>(b =>
            {
                b.ToTable("Ncc_UserLogin");
            });
            modelBuilder.Entity<IdentityUserToken<long>>(b =>
            {
                b.ToTable("Ncc_UserToken");
            });
            
        }
    }
}
