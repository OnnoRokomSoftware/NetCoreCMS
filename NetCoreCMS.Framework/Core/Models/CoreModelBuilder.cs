/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Utility;

namespace NetCoreCMS.Framework.Core.Models
{
    public class CoreModelBuilder : IModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {

            var NccModule = GlobalContext.GetTableName<NccModule>();
            var NccModuleDependency = GlobalContext.GetTableName<NccModuleDependency>();
            var NccMenu = GlobalContext.GetTableName<NccMenu>();
            var NccMenuItem = GlobalContext.GetTableName<NccMenuItem>();
            var NccPage = GlobalContext.GetTableName<NccPage>();
            var NccPageHistory = GlobalContext.GetTableName<NccPageHistory>();

            var NccPageDetails = GlobalContext.GetTableName<NccPageDetails>();
            var NccPageDetailsHistory = GlobalContext.GetTableName<NccPageDetailsHistory>();
            var NccPlugins = GlobalContext.GetTableName<NccPlugins>();
            var NccPost = GlobalContext.GetTableName<NccPost>();
            var NccCategory = GlobalContext.GetTableName<NccCategory>();
            var NccPostCategory = GlobalContext.GetTableName<NccPostCategory>();
            var NccComment = GlobalContext.GetTableName<NccComment>();
            var NccTag = GlobalContext.GetTableName<NccTag>();
            var NccPostTag = GlobalContext.GetTableName<NccPostTag>();
            var NccCategoryDetails = GlobalContext.GetTableName<NccCategoryDetails>();
            var NccPostDetails = GlobalContext.GetTableName<NccPostDetails>();
            var NccRole = GlobalContext.GetTableName<NccRole>();
            var NccSettings = GlobalContext.GetTableName<NccSettings>();
            var NccScheduleTaskHistory = GlobalContext.GetTableName<NccScheduleTaskHistory>();
            var NccStartup = GlobalContext.GetTableName<NccStartup>();
            var NccUser = GlobalContext.GetTableName<NccUser>();
            var NccUserRole = GlobalContext.GetTableName<NccUserRole>();
            var NccWebSite = GlobalContext.GetTableName<NccWebSite>();
            var NccWebSiteInfo = GlobalContext.GetTableName<NccWebSiteInfo>();

            var NccPermission = GlobalContext.GetTableName<NccPermission>();
            var NccPermissionDetails = GlobalContext.GetTableName<NccPermissionDetails>();
            var NccUserPermission = GlobalContext.GetTableName<NccUserPermission>();
            var NccWebSiteWidget = GlobalContext.GetTableName<NccWebSiteWidget>();
            var NccWidget = GlobalContext.GetTableName<NccWidget>();
            var NccWidgetSection = GlobalContext.GetTableName<NccWidgetSection>();
            var IdentityUserClaim = GlobalContext.GetTableName<IdentityUserClaim<long>>();
            var IdentityRoleClaim = GlobalContext.GetTableName<IdentityRoleClaim<long>>();
            var IdentityUserLogin = GlobalContext.GetTableName<IdentityUserLogin<long>>();
            var IdentityUserToken = GlobalContext.GetTableName<IdentityUserToken<long>>();


            modelBuilder.Entity<NccModule>(b => {
                b.ToTable(NccModule);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany(x => x.Dependencies);
            });

            modelBuilder.Entity<NccModuleDependency>(b => {
                b.ToTable(NccModuleDependency);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(x => x.NccModule);
            });

            modelBuilder.Entity<NccMenu>(b => {
                b.ToTable(NccMenu);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany(ur => ur.MenuItems);
            });

            modelBuilder.Entity<NccMenuItem>(b => {
                b.ToTable(NccMenuItem);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne( m => m.Parent );
                b.HasMany( m => m.SubActions );
                b.HasMany(m => m.Childrens);
            });
            
            modelBuilder.Entity<NccPage>(b => {
                b.ToTable(NccPage);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Parent);
            });

            modelBuilder.Entity<NccPageHistory>(b => {
                b.ToTable(NccPageHistory);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Parent);
                b.HasMany(p => p.PageDetailsHistory);
            });

            modelBuilder.Entity<NccPageDetails>(b => {
                b.ToTable(NccPageDetails);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Page);
            });

            modelBuilder.Entity<NccPageDetailsHistory>(b => {
                b.ToTable(NccPageDetailsHistory);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.PageHistory);
            });

            modelBuilder.Entity<NccPlugins>(b => {
                b.ToTable(NccPlugins);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany(p => p.Widgets);
            });

            modelBuilder.Entity<NccPost>(b => {

                b.ToTable(NccPost);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.Property(p => p.Id).ValueGeneratedOnAdd();
                b.HasOne(p => p.Parent);
                b.HasOne(p => p.Author);
                b.HasMany(p => p.PostDetails);
                b.HasMany(p => p.Categories);
                b.HasMany(p => p.Comments);
                b.HasMany(p => p.Tags);                
            });
            
            modelBuilder.Entity<NccCategory>(b => {
                b.ToTable(NccCategory);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Parent);
                b.HasMany(p => p.CategoryDetails);
                b.HasMany(p => p.Posts);
            });


            #region PostCatregories

            modelBuilder.Entity<NccPostCategory>()
                .ToTable(NccPostCategory)
                .HasKey(pt => new { pt.PostId, pt.CategoryId });

            modelBuilder.Entity<NccPostCategory>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.Categories)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<NccPostCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(t => t.Posts)
                .HasForeignKey(pt => pt.CategoryId); 

            #endregion


            modelBuilder.Entity<NccComment>(b => {
                b.ToTable(NccComment);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Post);
                b.HasOne(p => p.Author);
            });

            modelBuilder.Entity<NccTag>(b => {
                b.ToTable(NccTag);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany(p => p.Posts);
            });


            #region PostTags

            modelBuilder.Entity<NccPostTag>()
            .ToTable(NccPostTag)
            .HasKey(t => new { t.PostId, t.TagId });

            modelBuilder.Entity<NccPostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<NccPostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Posts)
                .HasForeignKey(pt => pt.TagId);

            #endregion

            modelBuilder.Entity<NccCategoryDetails>(b => {
                b.ToTable(NccCategoryDetails);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Category);
            });
            
            modelBuilder.Entity<NccPostDetails>(b => {
                b.ToTable(NccPostDetails);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasOne(p => p.Post);
            });

            modelBuilder.Entity<NccRole>(b =>
            {
                b.HasMany(ur => ur.Users);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.ToTable(NccRole);
            });

            modelBuilder.Entity<NccSettings>().ToTable(NccSettings).Property(x=>x.VersionNumber).IsConcurrencyToken();

            modelBuilder.Entity<NccScheduleTaskHistory>().ToTable(NccScheduleTaskHistory).Property(x=>x.VersionNumber).IsConcurrencyToken();

            modelBuilder.Entity<NccStartup>(b => {
                b.ToTable(NccStartup);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                //b.HasOne(p => p.User);
                b.HasOne(p => p.Permission);                
            });
             
            modelBuilder.Entity<NccUser>(b =>
            {
                b.ToTable(NccUser);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany(ur => ur.Roles);
                b.HasMany(w => w.ExtraPermissions);
                b.HasMany(w => w.ExtraDenies);
                b.HasMany(w => w.Permissions);
            });

            modelBuilder.Entity<NccUserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
                b.HasOne(ur => ur.Role).WithMany(r => r.Users).HasForeignKey(r => r.RoleId);
                b.HasOne(ur => ur.User).WithMany(u => u.Roles).HasForeignKey(u => u.UserId);
                b.ToTable(NccUserRole);
            });

            modelBuilder.Entity<NccWebSite>(b => {
                b.ToTable(NccWebSite);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany(w => w.WebSiteInfos);
                b.Property(p => p.EnableCache).HasDefaultValue(false);
            });

            modelBuilder.Entity<NccWebSiteInfo>().ToTable(NccWebSiteInfo).Property(x=>x.VersionNumber).IsConcurrencyToken();

            modelBuilder.Entity<NccPermission>(b => {
                b.ToTable(NccPermission);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.HasMany( x=> x.Users );
                b.HasMany(x => x.PermissionDetails);
            });

            modelBuilder.Entity<NccPermissionDetails>(b => {
                b.ToTable(NccPermissionDetails);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.Property(x => x.ExtraAllowUserId).IsRequired(false);
                b.Property(x => x.ExtraDenyUserId).IsRequired(false);
                b.Property(x => x.PermissionId).IsRequired(false);

                b.HasOne(w => w.Permission).WithMany(u=>u.PermissionDetails).HasForeignKey(y=>y.PermissionId);
                b.HasOne(w => w.DenyUser).WithMany(x => x.ExtraDenies).HasForeignKey(y => y.ExtraDenyUserId);
                b.HasOne(w => w.AllowUser).WithMany(x => x.ExtraPermissions).HasForeignKey(y => y.ExtraAllowUserId);                
            });

            modelBuilder.Entity<NccUserPermission>(b => {
                b.HasKey(up => new { up.UserId, up.PermissionId });
                b.ToTable(NccUserPermission);
                b.HasOne(w => w.User).WithMany( u => u.Permissions ).HasForeignKey(x=>x.UserId);
                b.HasOne(w => w.Permission).WithMany( u => u.Users ).HasForeignKey(x=>x.PermissionId);
            });

            modelBuilder.Entity<NccWebSiteWidget>(b => {
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
                b.ToTable(NccWebSiteWidget);
                b.HasOne(w => w.WebSite);
            });

            modelBuilder.Entity<NccWidget>().ToTable(NccWidget).Property(x=>x.VersionNumber).IsConcurrencyToken();
            modelBuilder.Entity<NccWidgetSection>(b => {
                b.ToTable(NccWidgetSection);
                b.Property(x => x.VersionNumber).IsConcurrencyToken();
            });

            modelBuilder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable(IdentityUserClaim);
            });
            modelBuilder.Entity<IdentityRoleClaim<long>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable(IdentityRoleClaim);
            });            
            modelBuilder.Entity<IdentityUserLogin<long>>(b =>
            {
                b.ToTable(IdentityUserLogin);
            });
            modelBuilder.Entity<IdentityUserToken<long>>(b =>
            {
                b.ToTable(IdentityUserToken);
            });            
        }
    }
}
