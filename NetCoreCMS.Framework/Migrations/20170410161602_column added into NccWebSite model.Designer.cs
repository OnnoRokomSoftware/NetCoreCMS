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
    [Migration("20170410161602_column added into NccWebSite model")]
    partial class columnaddedintoNccWebSitemodel
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

                    b.Property<int>("MenuType");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Name");

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

                    b.Property<int>("ActionType");

                    b.Property<string>("Controller");

                    b.Property<long>("CreateBy");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Data");

                    b.Property<string>("MenuIconCls");

                    b.Property<DateTime>("ModificationDate");

                    b.Property<long>("ModifyBy");

                    b.Property<string>("Module");

                    b.Property<string>("Name");

                    b.Property<long?>("NccMenuId");

                    b.Property<long?>("ParentId");

                    b.Property<int>("Position");

                    b.Property<int>("Status");

                    b.Property<string>("Target");

                    b.Property<int>("VersionNumber");

                    b.HasKey("Id");

                    b.HasIndex("NccMenuId");

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

                    b.Property<int>("Status");

                    b.Property<string>("ThemeName");

                    b.Property<string>("Type");

                    b.Property<string>("Version");

                    b.Property<int>("VersionNumber");

                    b.Property<string>("Website");

                    b.HasKey("Id");

                    b.ToTable("Ncc_Theme");
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

                    b.HasOne("NetCoreCMS.Framework.Core.Models.NccMenuItem", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
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
        }
    }
}
