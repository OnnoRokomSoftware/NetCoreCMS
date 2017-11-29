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

namespace NetCoreCMS.Framework.Core.Models
{
    public class CoreModelBuilder : IModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NccModule>(b => {
                b.ToTable("Ncc_Module");
            });

            modelBuilder.Entity<NccModuleDependency>(b => {
                b.ToTable("Ncc_Module_Dependency");
            });

            modelBuilder.Entity<NccMenu>(b => {
                b.ToTable("Ncc_Menu");
                b.HasMany(ur => ur.MenuItems);
            });

            modelBuilder.Entity<NccMenuItem>(b => {
                b.ToTable("Ncc_Menu_Item");
                b.HasOne( m => m.Parent );
                b.HasMany( m => m.SubActions );
                b.HasMany(m => m.Childrens);
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

                b.ToTable("Ncc_Post");
                b.Property(p => p.Id).ValueGeneratedOnAdd();
                b.HasOne(p => p.Parent);
                b.HasOne(p => p.Author);
                b.HasMany(p => p.PostDetails);
                b.HasMany(p => p.Categories);
                b.HasMany(p => p.Comments);
                b.HasMany(p => p.Tags);                
            });
            
            modelBuilder.Entity<NccCategory>(b => {
                b.ToTable("Ncc_Category");
                b.HasOne(p => p.Parent);
                b.HasMany(p => p.CategoryDetails);
                b.HasMany(p => p.Posts);
            });


            #region PostCatregories

            modelBuilder.Entity<NccPostCategory>()
                .ToTable("Ncc_Post_Category")
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
                b.ToTable("Ncc_Post_Comment");
                b.HasOne(p => p.Post);
                b.HasOne(p => p.Author);
            });

            modelBuilder.Entity<NccTag>(b => {
                b.ToTable("Ncc_Tag");
                b.HasMany(p => p.Posts);
            });


            #region PostTags

            modelBuilder.Entity<NccPostTag>()
            .ToTable("Ncc_Post_Tag")
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
                b.ToTable("Ncc_Category_Details");
                b.HasOne(p => p.Category);
            });

            modelBuilder.Entity<NccPageDetails>(b => {
                b.ToTable("Ncc_Page_Details");
                b.HasOne(p => p.Page);
            });

            modelBuilder.Entity<NccPostDetails>(b => {
                b.ToTable("Ncc_Post_Details");
                b.HasOne(p => p.Post);
            });

            modelBuilder.Entity<NccRole>(b =>
            {
                b.HasMany(ur => ur.Users);
                b.ToTable("Ncc_Role");
            });

            modelBuilder.Entity<NccSettings>().ToTable("Ncc_Settings");

            modelBuilder.Entity<NccScheduleTaskHistory>().ToTable("Ncc_Schedule_Task_History");

            modelBuilder.Entity<NccStartup>(b => {
                b.ToTable("Ncc_Startup");
                //b.HasOne(p => p.User);
                b.HasOne(p => p.Permission);                
            });
             
            modelBuilder.Entity<NccUser>(b =>
            {
                b.ToTable("Ncc_User");
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
                b.ToTable("Ncc_User_Role");
            });

            modelBuilder.Entity<NccWebSite>(b => {
                b.ToTable("Ncc_WebSite");
                b.HasMany(w => w.WebSiteInfos);
                b.Property(p => p.EnableCache).HasDefaultValue(false);
            });

            modelBuilder.Entity<NccWebSiteInfo>().ToTable("Ncc_WebSite_Info");

            modelBuilder.Entity<NccPermission>(b => {
                b.ToTable("Ncc_Permission");
                b.HasMany( x=> x.Users );
                b.HasMany(x => x.PermissionDetails);
            });

            modelBuilder.Entity<NccPermissionDetails>(b => {
                b.ToTable("Ncc_Permission_Details");
                b.Property(x => x.ExtraAllowUserId).IsRequired(false);
                b.Property(x => x.ExtraDenyUserId).IsRequired(false);
                b.Property(x => x.PermissionId).IsRequired(false);

                b.HasOne(w => w.Permission).WithMany(u=>u.PermissionDetails).HasForeignKey(y=>y.PermissionId);
                b.HasOne(w => w.DenyUser).WithMany(x => x.ExtraDenies).HasForeignKey(y => y.ExtraDenyUserId);
                b.HasOne(w => w.AllowUser).WithMany(x => x.ExtraPermissions).HasForeignKey(y => y.ExtraAllowUserId);                
            });

            modelBuilder.Entity<NccUserPermission>(b => {
                b.HasKey(up => new { up.UserId, up.PermissionId });
                b.ToTable("Ncc_User_Permission");
                b.HasOne(w => w.User).WithMany( u => u.Permissions ).HasForeignKey(x=>x.UserId);
                b.HasOne(w => w.Permission).WithMany( u => u.Users ).HasForeignKey(x=>x.PermissionId);
            });

            modelBuilder.Entity<NccWebSiteWidget>(b => {
                b.ToTable("Ncc_WebSite_Widget");
                b.HasOne(w => w.WebSite);
            });

            modelBuilder.Entity<NccWidget>().ToTable("Ncc_Widget");
            modelBuilder.Entity<NccWidgetSection>(b => {
                b.ToTable("Ncc_Widget_Sections");
            });

            modelBuilder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("Ncc_User_Claim");
            });
            modelBuilder.Entity<IdentityRoleClaim<long>>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.ToTable("Ncc_Role_Claim");
            });            
            modelBuilder.Entity<IdentityUserLogin<long>>(b =>
            {
                b.ToTable("Ncc_User_Login");
            });
            modelBuilder.Entity<IdentityUserToken<long>>(b =>
            {
                b.ToTable("Ncc_User_Token");
            });            
        }
    }
}
