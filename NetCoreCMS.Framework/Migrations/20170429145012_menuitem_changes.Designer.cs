using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreCMS.Framework.Core.Data;
using NetCoreCMS.Framework.Core.Models;

namespace NetCoreCMS.Framework.Migrations
{
    [DbContext(typeof(NccDbContext))]
    [Migration("20170429145012_menuitem_changes")]
    partial class menuitem_changes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Ncc_RoleClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Ncc_UserClaim");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("Ncc_UserLogin");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("Ncc_UserToken");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccMenu", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("MenuFor");

                    b.Property<string>("MenuIconCls");

                    b.Property<int>("MenuOrder");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<int>("Position");

                    b.Property<int>("Status");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.ToTable("Ncc_NccMenu");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccMenuItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<string>("Controller");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Data");

                    b.Property<int>("MenuActionType");

                    b.Property<int>("MenuFor");

                    b.Property<string>("MenuIconCls");

                    b.Property<int>("MenuOrder");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Module");

                    b.Property<string>("Name");

                    b.Property<long?>("NccMenuId");

                    b.Property<long?>("NccMenuItemId");

                    b.Property<long?>("NccMenuItemId1");

                    b.Property<long?>("ParentId");

                    b.Property<int>("Position");

                    b.Property<int>("Status");

                    b.Property<string>("Target");

                    b.Property<string>("Url");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NccMenuId");

                    b.HasIndex("NccMenuItemId");

                    b.HasIndex("NccMenuItemId1");

                    b.HasIndex("ParentId");

                    b.ToTable("Ncc_MenuItem");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccModule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AntiForgery");

                    b.Property<string>("Author");

                    b.Property<string>("Category");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<int>("ModuleStatus");

                    b.Property<string>("Name");

                    b.Property<string>("NetCoreCMSVersion");

                    b.Property<string>("Path");

                    b.Property<string>("SortName");

                    b.Property<int>("Status");

                    b.Property<string>("Version");

                    b.Property<int>("VersionNumber");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Ncc_Module");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AddToNavigationMenu");

                    b.Property<byte[]>("Content");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("MetaDescription");

                    b.Property<string>("MetaKeyword");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<int>("PageOrder");

                    b.Property<int>("PageStatus");

                    b.Property<int>("PageType");

                    b.Property<long?>("ParentId");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Ncc_Page");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPlugins", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AntiForgery");

                    b.Property<string>("Author");

                    b.Property<string>("Category");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("NetCoreCMSVersion");

                    b.Property<string>("Path");

                    b.Property<int>("PluginsStatus");

                    b.Property<string>("SortName");

                    b.Property<int>("Status");

                    b.Property<string>("Version");

                    b.Property<int>("VersionNumber");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Ncc_Plugins");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPost", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowComment");

                    b.Property<long?>("AuthorId");

                    b.Property<byte[]>("Content");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<bool>("IsFeatured");

                    b.Property<bool>("IsStiky");

                    b.Property<string>("MetaDescription");

                    b.Property<string>("MetaKeyword");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("NccPostCategoryId");

                    b.Property<long?>("ParentId");

                    b.Property<int>("PostStatus");

                    b.Property<int>("PostType");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Slug");

                    b.Property<int>("Status");

                    b.Property<string>("ThumImage");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("NccPostCategoryId");

                    b.HasIndex("ParentId");

                    b.ToTable("Ncc_NccPost");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPostCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryImage");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("MetaDescription");

                    b.Property<string>("MetaKeyword");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("NccPostId");

                    b.Property<long?>("ParentId");

                    b.Property<string>("Slug");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NccPostId");

                    b.HasIndex("ParentId");

                    b.ToTable("Ncc_PostCategory");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPostComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AuthorId");

                    b.Property<byte[]>("Content");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("PostId");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("Ncc_PostComment");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<string>("Slug");

                    b.Property<int>("Status");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Ncc_Role");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccSettings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<string>("Value");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.ToTable("Ncc_Settings");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("NccPostId");

                    b.Property<int>("Status");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NccPostId");

                    b.ToTable("Ncc_Tag");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccTheme", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Category");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("NetCoreCMSVersion");

                    b.Property<string>("PreviewImage");

                    b.Property<int>("Status");

                    b.Property<string>("ThemeName");

                    b.Property<int>("Type");

                    b.Property<string>("Version");

                    b.Property<int>("VersionNumber");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Ncc_Theme");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccThemeLayout", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<string>("FileName");

                    b.Property<string>("LayoutImage");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("NetCoreCMSVersion");

                    b.Property<int>("Status");

                    b.Property<long?>("ThemeId");

                    b.Property<string>("Version");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("ThemeId");

                    b.ToTable("Ncc_ThemeLayout");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Mobile");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Slug");

                    b.Property<int>("Status");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("Ncc_User");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccUserRole", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("Ncc_UserRole");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWebSite", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowRegistration");

                    b.Property<string>("Copyrights");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("DateFormat");

                    b.Property<string>("DomainName");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FaviconUrl");

                    b.Property<string>("GoogleAnalyticsId");

                    b.Property<string>("Language");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<string>("NewUserRole");

                    b.Property<string>("PrivacyPolicyUrl");

                    b.Property<string>("SiteLogoUrl");

                    b.Property<string>("SiteTitle")
                        .IsRequired();

                    b.Property<int>("Status");

                    b.Property<string>("Tagline");

                    b.Property<string>("TermsAndConditionsUrl");

                    b.Property<string>("TimeFormat");

                    b.Property<string>("TimeZone");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.ToTable("Ncc_WebSite");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWebSiteWidget", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<long?>("LayoutId");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("SectionId");

                    b.Property<int>("Status");

                    b.Property<long?>("ThemeId");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.Property<long?>("WebSiteId");

                    b.Property<string>("WidgetConfigJson");

                    b.Property<long?>("WidgetId");

                    b.Property<int>("WidgetOrder");

                    b.HasKey("Id");

                    b.HasIndex("LayoutId");

                    b.HasIndex("SectionId");

                    b.HasIndex("ThemeId");

                    b.HasIndex("WebSiteId");

                    b.HasIndex("WidgetId");

                    b.ToTable("Ncc_WebSiteWidget");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWidget", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("NccModuleId");

                    b.Property<long?>("NccPluginsId");

                    b.Property<string>("NetCoreCMSVersion");

                    b.Property<string>("SortName");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NccModuleId");

                    b.HasIndex("NccPluginsId");

                    b.ToTable("Ncc_Widget");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWidgetSection", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Dependencies");

                    b.Property<string>("Description");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

                    b.Property<long?>("NccThemeLayoutId");

                    b.Property<string>("NetCoreCMSVersion");

                    b.Property<string>("SortName");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NccThemeLayoutId");

                    b.ToTable("Ncc_WidgetSections");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccMenuItem", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccMenu")
                        .WithMany("MenuItems")
                        .HasForeignKey("NccMenuId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccMenuItem")
                        .WithMany("SubActions")
                        .HasForeignKey("NccMenuItemId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccMenuItem")
                        .WithMany("Childrens")
                        .HasForeignKey("NccMenuItemId1");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccMenuItem", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPage", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPage", "Parent")
                        .WithMany("LinkedPages")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPost", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPostCategory")
                        .WithMany("Posts")
                        .HasForeignKey("NccPostCategoryId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPost", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPostCategory", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPost")
                        .WithMany("Categories")
                        .HasForeignKey("NccPostId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPostCategory", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccPostComment", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPost", "Post")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccTag", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPost")
                        .WithMany("Tags")
                        .HasForeignKey("NccPostId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccThemeLayout", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccTheme", "Theme")
                        .WithMany("ThemeLayouts")
                        .HasForeignKey("ThemeId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccUserRole", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccUser", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWebSiteWidget", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccThemeLayout", "Layout")
                        .WithMany()
                        .HasForeignKey("LayoutId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccWidgetSection", "Section")
                        .WithMany()
                        .HasForeignKey("SectionId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccTheme", "Theme")
                        .WithMany()
                        .HasForeignKey("ThemeId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccWebSite", "WebSite")
                        .WithMany()
                        .HasForeignKey("WebSiteId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccWidget", "Widget")
                        .WithMany()
                        .HasForeignKey("WidgetId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWidget", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccModule")
                        .WithMany("Widgets")
                        .HasForeignKey("NccModuleId");

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccPlugins")
                        .WithMany("Widgets")
                        .HasForeignKey("NccPluginsId");
                });

            modelBuilder.Entity("NetCoreCMS.Framework.Core.Models.NccWidgetSection", b =>
                {
                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccThemeLayout")
                        .WithMany("WidgetSections")
                        .HasForeignKey("NccThemeLayoutId");
                });
        }
    }
}
