using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class initiMySql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ncc_UserToken",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Ncc_NccMenu",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    MenuIconCls = table.Column<string>(nullable: true),
                    MenuOrder = table.Column<int>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_NccMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Module",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AntiForgery = table.Column<bool>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleId = table.Column<string>(nullable: true),
                    ModuleStatus = table.Column<int>(nullable: false),
                    ModuleTitle = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Page",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AddToNavigationMenu = table.Column<bool>(nullable: false),
                    Content = table.Column<string>(maxLength: 2147483647, nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Layout = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PageOrder = table.Column<int>(nullable: false),
                    PageStatus = table.Column<int>(nullable: false),
                    PageType = table.Column<int>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Page_Ncc_Page_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Plugins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AntiForgery = table.Column<bool>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    PluginsStatus = table.Column<int>(nullable: false),
                    SortName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Plugins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Settings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Theme",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    PreviewImage = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ThemeId = table.Column<string>(nullable: true),
                    ThemeName = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_WebSite",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AllowRegistration = table.Column<bool>(nullable: false),
                    Copyrights = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateFormat = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    FaviconUrl = table.Column<string>(nullable: true),
                    GoogleAnalyticsId = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NewUserRole = table.Column<string>(nullable: true),
                    PrivacyPolicyUrl = table.Column<string>(nullable: true),
                    SiteLogoUrl = table.Column<string>(nullable: true),
                    SiteTitle = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Tagline = table.Column<string>(nullable: true),
                    TermsAndConditionsUrl = table.Column<string>(nullable: true),
                    TimeFormat = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_WebSite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_MenuItem",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Action = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    MenuActionType = table.Column<int>(nullable: false),
                    MenuFor = table.Column<int>(nullable: false),
                    MenuIconCls = table.Column<string>(nullable: true),
                    MenuOrder = table.Column<int>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Module = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NccMenuId = table.Column<long>(nullable: true),
                    NccMenuItemId = table.Column<long>(nullable: true),
                    NccMenuItemId1 = table.Column<long>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Target = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_MenuItem_Ncc_NccMenu_NccMenuId",
                        column: x => x.NccMenuId,
                        principalTable: "Ncc_NccMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_MenuItem_Ncc_MenuItem_NccMenuItemId",
                        column: x => x.NccMenuItemId,
                        principalTable: "Ncc_MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_MenuItem_Ncc_MenuItem_NccMenuItemId1",
                        column: x => x.NccMenuItemId1,
                        principalTable: "Ncc_MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_MenuItem_Ncc_MenuItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Widget",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    Content = table.Column<byte[]>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NccPluginsId = table.Column<long>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    SortName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Widget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Widget_Ncc_Plugins_NccPluginsId",
                        column: x => x.NccPluginsId,
                        principalTable: "Ncc_Plugins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_RoleClaim_Ncc_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ncc_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_ThemeLayout",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    LayoutImage = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ThemeId = table.Column<long>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_ThemeLayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_ThemeLayout_Ncc_Theme_ThemeId",
                        column: x => x.ThemeId,
                        principalTable: "Ncc_Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_UserClaim_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Ncc_UserLogin_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_UserRole",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Ncc_UserRole_Ncc_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ncc_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_UserRole_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_WebSiteWidget",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LayoutName = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ThemeId = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    WebSiteId = table.Column<long>(nullable: true),
                    WidgetConfigJson = table.Column<string>(nullable: true),
                    WidgetData = table.Column<string>(nullable: true),
                    WidgetId = table.Column<string>(nullable: true),
                    WidgetOrder = table.Column<int>(nullable: false),
                    Zone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_WebSiteWidget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_WebSiteWidget_Ncc_WebSite_WebSiteId",
                        column: x => x.WebSiteId,
                        principalTable: "Ncc_WebSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_WidgetSections",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NccThemeLayoutId = table.Column<long>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    SortName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_WidgetSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_WidgetSections_Ncc_ThemeLayout_NccThemeLayoutId",
                        column: x => x.NccThemeLayoutId,
                        principalTable: "Ncc_ThemeLayout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_PostCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CategoryImage = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NccPostId = table.Column<long>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_PostCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_PostCategory_Ncc_PostCategory_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_PostCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_NccPost",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AllowComment = table.Column<bool>(nullable: false),
                    AuthorId = table.Column<long>(nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsStiky = table.Column<bool>(nullable: false),
                    Layout = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NccPostCategoryId = table.Column<long>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    PostStatus = table.Column<int>(nullable: false),
                    PostType = table.Column<int>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ThumImage = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_NccPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_NccPost_Ncc_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_NccPost_Ncc_PostCategory_NccPostCategoryId",
                        column: x => x.NccPostCategoryId,
                        principalTable: "Ncc_PostCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_NccPost_Ncc_NccPost_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_NccPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_PostComment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    AuthorId = table.Column<long>(nullable: true),
                    Content = table.Column<byte[]>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_PostComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_PostComment_Ncc_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_PostComment_Ncc_NccPost_PostId",
                        column: x => x.PostId,
                        principalTable: "Ncc_NccPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Tag",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NccPostId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Tag_Ncc_NccPost_NccPostId",
                        column: x => x.NccPostId,
                        principalTable: "Ncc_NccPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_RoleClaim_RoleId",
                table: "Ncc_RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_UserClaim_UserId",
                table: "Ncc_UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_UserLogin_UserId",
                table: "Ncc_UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_MenuItem_NccMenuId",
                table: "Ncc_MenuItem",
                column: "NccMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_MenuItem_NccMenuItemId",
                table: "Ncc_MenuItem",
                column: "NccMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_MenuItem_NccMenuItemId1",
                table: "Ncc_MenuItem",
                column: "NccMenuItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_MenuItem_ParentId",
                table: "Ncc_MenuItem",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Page_ParentId",
                table: "Ncc_Page",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_NccPost_AuthorId",
                table: "Ncc_NccPost",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_NccPost_NccPostCategoryId",
                table: "Ncc_NccPost",
                column: "NccPostCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_NccPost_ParentId",
                table: "Ncc_NccPost",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_PostCategory_NccPostId",
                table: "Ncc_PostCategory",
                column: "NccPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_PostCategory_ParentId",
                table: "Ncc_PostCategory",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_PostComment_AuthorId",
                table: "Ncc_PostComment",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_PostComment_PostId",
                table: "Ncc_PostComment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Ncc_Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Tag_NccPostId",
                table: "Ncc_Tag",
                column: "NccPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_ThemeLayout_ThemeId",
                table: "Ncc_ThemeLayout",
                column: "ThemeId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Ncc_User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Ncc_User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_UserRole_RoleId",
                table: "Ncc_UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_WebSiteWidget_WebSiteId",
                table: "Ncc_WebSiteWidget",
                column: "WebSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Widget_NccPluginsId",
                table: "Ncc_Widget",
                column: "NccPluginsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_WidgetSections_NccThemeLayoutId",
                table: "Ncc_WidgetSections",
                column: "NccThemeLayoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ncc_PostCategory_Ncc_NccPost_NccPostId",
                table: "Ncc_PostCategory",
                column: "NccPostId",
                principalTable: "Ncc_NccPost",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ncc_NccPost_Ncc_User_AuthorId",
                table: "Ncc_NccPost");

            migrationBuilder.DropForeignKey(
                name: "FK_Ncc_NccPost_Ncc_PostCategory_NccPostCategoryId",
                table: "Ncc_NccPost");

            migrationBuilder.DropTable(
                name: "Ncc_RoleClaim");

            migrationBuilder.DropTable(
                name: "Ncc_UserClaim");

            migrationBuilder.DropTable(
                name: "Ncc_UserLogin");

            migrationBuilder.DropTable(
                name: "Ncc_UserToken");

            migrationBuilder.DropTable(
                name: "Ncc_MenuItem");

            migrationBuilder.DropTable(
                name: "Ncc_Module");

            migrationBuilder.DropTable(
                name: "Ncc_Page");

            migrationBuilder.DropTable(
                name: "Ncc_PostComment");

            migrationBuilder.DropTable(
                name: "Ncc_Settings");

            migrationBuilder.DropTable(
                name: "Ncc_Tag");

            migrationBuilder.DropTable(
                name: "Ncc_UserRole");

            migrationBuilder.DropTable(
                name: "Ncc_WebSiteWidget");

            migrationBuilder.DropTable(
                name: "Ncc_Widget");

            migrationBuilder.DropTable(
                name: "Ncc_WidgetSections");

            migrationBuilder.DropTable(
                name: "Ncc_NccMenu");

            migrationBuilder.DropTable(
                name: "Ncc_Role");

            migrationBuilder.DropTable(
                name: "Ncc_WebSite");

            migrationBuilder.DropTable(
                name: "Ncc_Plugins");

            migrationBuilder.DropTable(
                name: "Ncc_ThemeLayout");

            migrationBuilder.DropTable(
                name: "Ncc_Theme");

            migrationBuilder.DropTable(
                name: "Ncc_User");

            migrationBuilder.DropTable(
                name: "Ncc_PostCategory");

            migrationBuilder.DropTable(
                name: "Ncc_NccPost");
        }
    }
}
