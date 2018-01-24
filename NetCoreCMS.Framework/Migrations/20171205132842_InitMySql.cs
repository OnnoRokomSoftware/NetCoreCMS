using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using NetCoreCMS.Framework.Utility;
using NetCoreCMS.Framework.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class InitMySql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: NccCategory,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryImage = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccCategory}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccCategory}_{NccCategory}_ParentId",
                        column: x => x.ParentId,
                        principalTable: NccCategory,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccMenu,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    MenuIconCls = table.Column<string>(nullable: true),
                    MenuLanguage = table.Column<string>(nullable: true),
                    MenuOrder = table.Column<int>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccMenu}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccModule,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ExecutionOrder = table.Column<int>(nullable: false, defaultValue: 100),
                    AntiForgery = table.Column<bool>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Folder = table.Column<string>(nullable: true),
                    IsCore = table.Column<bool>(nullable: false),                    
                    Metadata = table.Column<string>(nullable: true),
                    NccVersion = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleName = table.Column<string>(nullable: true),
                    ModuleStatus = table.Column<int>(nullable: false),
                    ModuleTitle = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccModule}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccPage,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Layout = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PageOrder = table.Column<int>(nullable: false),
                    PageStatus = table.Column<int>(nullable: false),
                    PageType = table.Column<int>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPage}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPage}_{NccPage}_ParentId",
                        column: x => x.ParentId,
                        principalTable: NccPage,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccPermission,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Rank = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPermission}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccPlugins,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AntiForgery = table.Column<bool>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
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
                    table.PrimaryKey($"PK_{NccPlugins}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccRole,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
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
                    table.PrimaryKey($"PK_{NccRole}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccScheduleTaskHistory,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TaskCreator = table.Column<string>(nullable: true),
                    TaskId = table.Column<string>(nullable: true),
                    TaskOf = table.Column<string>(nullable: true),
                    TaskType = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccScheduleTaskHistory}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccSettings,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    GroupId = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccSettings}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccTag,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccTag}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccUser,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    IsRequireLogin = table.Column<bool>(nullable: false, defaultValue:false),
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
                    table.PrimaryKey($"PK_{NccUser}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccWebSite,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AdminPageSize = table.Column<int>(nullable: false),
                    AllowRegistration = table.Column<bool>(nullable: false),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateFormat = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    EnableCache = table.Column<bool>(nullable: false, defaultValue: false),
                    GoogleAnalyticsId = table.Column<string>(nullable: true),
                    IsMultiLangual = table.Column<bool>(nullable: false),
                    IsShowFullPost = table.Column<bool>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NewUserRole = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TablePrefix = table.Column<string>(nullable: true),
                    TimeFormat = table.Column<string>(nullable: true),
                    TimeZone = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    WebSitePageSize = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccWebSite}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccWidgetSection,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    SortName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccWidgetSection}", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: NccCategoryDetails,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccCategoryDetails}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccCategoryDetails}_{NccCategoryDetails}_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: NccCategoryDetails,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccMenuItem,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    MenuActionType = table.Column<int>(nullable: false),
                    MenuFor = table.Column<int>(nullable: false),
                    MenuIconCls = table.Column<string>(nullable: true),
                    MenuOrder = table.Column<int>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
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
                    IsAnonymous = table.Column<bool>(nullable: false, defaultValue:false),
                    IsAllowAuthenticated = table.Column<bool>(nullable: false, defaultValue: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccMenuItem}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccMenuItem}_{NccMenu}_NccMenuId",
                        column: x => x.NccMenuId,
                        principalTable: NccMenu,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccMenuItem}_{NccMenuItem}_NMIId",
                        column: x => x.NccMenuItemId,
                        principalTable: NccMenuItem,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccMenuItem}_{NccMenuItem}_NMItemId1",
                        column: x => x.NccMenuItemId1,
                        principalTable: NccMenuItem,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccMenuItem}_{NccMenuItem}_ParentId",
                        column: x => x.ParentId,
                        principalTable: NccMenuItem,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccModuleDependency,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),                    
                    Metadata = table.Column<string>(nullable: true),
                    ModuleVersion = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NccModuleId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccModuleDependency}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccModuleDependency}_{NccModule}_NccModuleId",
                        column: x => x.NccModuleId,
                        principalTable: NccModule,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccPageDetails,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(maxLength: 2147483647, nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PageId = table.Column<long>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPageDetails}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPageDetails}_{NccPage}_PageId",
                        column: x => x.PageId,
                        principalTable: NccPage,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccPageHistory,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Layout = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PageId = table.Column<long>(nullable: false),
                    PageOrder = table.Column<int>(nullable: false),
                    PageStatus = table.Column<int>(nullable: false),
                    PageType = table.Column<int>(nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPageHistory}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPageHistory}_{NccPage}_ParentId",
                        column: x => x.ParentId,
                        principalTable: NccPage,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccStartup,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PermissionId = table.Column<long>(nullable: true),
                    RoleId = table.Column<long>(nullable: false),
                    StartupFor = table.Column<int>(nullable: false),
                    StartupType = table.Column<int>(nullable: false),
                    StartupUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_NccStartup", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccStartup}_{NccPermission}_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: NccPermission,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccWidget,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<byte[]>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
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
                    table.PrimaryKey($"PK_{NccWidget}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccWidget}_{NccPlugins}_NccPluginsId",
                        column: x => x.NccPluginsId,
                        principalTable: NccPlugins,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: IdentityRoleClaim,
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{IdentityRoleClaim}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{IdentityRoleClaim}_{NccRole}_RoleId",
                        column: x => x.RoleId,
                        principalTable: NccRole,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: NccPermissionDetails,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ExtraAllowUserId = table.Column<long>(nullable: true),
                    ExtraDenyUserId = table.Column<long>(nullable: true),
                    MenuType = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    PermissionId = table.Column<long>(nullable: true),
                    Requirements = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPermissionDetails}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPermissionDetails}_{NccUser}_ExtraAllowUserId",
                        column: x => x.ExtraAllowUserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccPermissionDetails}_{NccUser}_ExtraDenyUserId",
                        column: x => x.ExtraDenyUserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccPermissionDetails}_{NccPermission}_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: NccPermission,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: NccPost,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllowComment = table.Column<bool>(nullable: false),
                    AuthorId = table.Column<long>(nullable: true),
                    CommentCount = table.Column<long>(nullable: false),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsStiky = table.Column<bool>(nullable: false),
                    Layout = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    PostStatus = table.Column<int>(nullable: false),
                    PostType = table.Column<int>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    RelatedPosts = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ThumImage = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPost}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPost}_{NccUser}_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccPost}_{NccPost}_ParentId",
                        column: x => x.ParentId,
                        principalTable: NccPost,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccUserPermission,
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    PermissionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccUserPermission}", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: $"FK_{NccUserPermission}_{NccPermission}_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: NccPermission,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate: ReferentialAction.Cascade
                        );
                    table.ForeignKey(
                        name: $"FK_{NccUserPermission}_{NccUser}_UserId",
                        column: x => x.UserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade,
                        onUpdate:ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: NccUserRole,
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccUserRole}", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: $"FK_{NccUserRole}_{NccRole}_RoleId",
                        column: x => x.RoleId,
                        principalTable: NccRole,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: $"FK_{NccUserRole}_{NccUser}_UserId",
                        column: x => x.UserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: IdentityUserClaim,
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    NccUserId = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{IdentityUserClaim}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{IdentityUserClaim}_{NccUser}_NccUserId",
                        column: x => x.NccUserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{IdentityUserClaim}_{NccUser}_UserId",
                        column: x => x.UserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: IdentityUserLogin,
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    NccUserId = table.Column<long>(nullable: true),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{IdentityUserLogin}", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: $"FK_{IdentityUserLogin}_{NccUser}_NccUserId",
                        column: x => x.NccUserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{IdentityUserLogin}_{NccUser}_UserId",
                        column: x => x.UserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: IdentityUserToken,
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{IdentityUserToken}", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: $"FK_{IdentityUserToken}_{NccUser}_UserId",
                        column: x => x.UserId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: NccWebSiteInfo,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Copyrights = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FaviconUrl = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NccWebSiteId = table.Column<long>(nullable: true),
                    PrivacyPolicyUrl = table.Column<string>(nullable: true),
                    SiteLogoUrl = table.Column<string>(nullable: true),
                    SiteTitle = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tagline = table.Column<string>(nullable: true),
                    TermsAndConditionsUrl = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccWebSiteInfo}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccWebSiteInfo}_{NccWebSite}_NccWebSiteId",
                        column: x => x.NccWebSiteId,
                        principalTable: NccWebSite,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccWebSiteWidget,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LayoutName = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleName = table.Column<string>(nullable: true),
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
                    table.PrimaryKey($"PK_{NccWebSiteWidget}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccWebSiteWidget}_{NccWebSite}_WebSiteId",
                        column: x => x.WebSiteId,
                        principalTable: NccWebSite,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccPageDetailsHistory,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(maxLength: 2147483647, nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PageDetailsId = table.Column<long>(nullable: false),
                    PageHistoryId = table.Column<long>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPageDetailsHistory}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPageDetailsHistory}_{NccPageDetailsHistory}_PageHistoryId",
                        column: x => x.PageHistoryId,
                        principalTable: NccPageDetailsHistory,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccComment,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<long>(nullable: true),
                    AuthorName = table.Column<string>(nullable: true),
                    CommentStatus = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccComment}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccComment}_{NccUser}_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: NccUser,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: $"FK_{NccComment}_{NccPost}_PostId",
                        column: x => x.PostId,
                        principalTable: NccPost,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccPostCategory,
                columns: table => new
                {
                    PostId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPostCategory}", x => new { x.PostId, x.CategoryId });
                    table.ForeignKey(
                        name: $"FK_{NccPostCategory}_{NccCategory}_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: NccCategory,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: $"FK_{NccPostCategory}_{NccPost}_PostId",
                        column: x => x.PostId,
                        principalTable: NccPost,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: NccPostDetails,
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(maxLength: 2147483647, nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Language = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeyword = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PostId = table.Column<long>(nullable: true),
                    Slug = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPostDetails}", x => x.Id);
                    table.ForeignKey(
                        name: $"FK_{NccPostDetails}_{NccPost}_PostId",
                        column: x => x.PostId,
                        principalTable: NccPost,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: NccPostTag,
                columns: table => new
                {
                    PostId = table.Column<long>(nullable: false),
                    TagId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey($"PK_{NccPostTag}", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: $"FK_{NccPostTag}_{NccPost}_PostId",
                        column: x => x.PostId,
                        principalTable: NccPost,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: $"FK_{NccPostTag}_{NccTag}_TagId",
                        column: x => x.TagId,
                        principalTable: NccTag,
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: $"IX_{NccCategory}_ParentId",
                table: NccCategory,
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccCategoryDetails}_CategoryId",
                table: NccCategoryDetails,
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccComment}_AuthorId",
                table: NccComment,
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccComment}_PostId",
                table: NccComment,
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccMenuItem}_NccMenuId",
                table: NccMenuItem,
                column: "NccMenuId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccMenuItem}_NccMenuItemId",
                table: NccMenuItem,
                column: "NccMenuItemId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccMenuItem}_NccMenuItemId1",
                table: NccMenuItem,
                column: "NccMenuItemId1");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccMenuItem}_ParentId",
                table: NccMenuItem,
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccModuleDependency}_NccModuleId",
                table: NccModuleDependency,
                column: "NccModuleId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPage}_ParentId",
                table: NccPage,
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPageDetails}_PageId",
                table: NccPageDetails,
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPageDetailsHistory}_PageHistoryId",
                table: NccPageDetailsHistory,
                column: "PageHistoryId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPageHistory}_ParentId",
                table: NccPageHistory,
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPermissionDetails}_ExtraAllowUserId",
                table: NccPermissionDetails,
                column: "ExtraAllowUserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPermissionDetails}_ExtraDenyUserId",
                table: NccPermissionDetails,
                column: "ExtraDenyUserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPermissionDetails}_PermissionId",
                table: NccPermissionDetails,
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPost}_AuthorId",
                table: NccPost,
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPost}_ParentId",
                table: NccPost,
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPostCategory}_CategoryId",
                table: NccPostCategory,
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPostDetails}_PostId",
                table: NccPostDetails,
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccPostTag}_TagId",
                table: NccPostTag,
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccRole}_RoleNameIndex",
                table: NccRole,
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: $"IX_{NccStartup}_PermissionId",
                table: NccStartup,
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccUser}_EmailIndex",
                table: NccUser,
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccUser}_UserNameIndex",
                table: NccUser,
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: $"IX_{NccUserPermission}_PermissionId",
                table: NccUserPermission,
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccUserRole}_RoleId",
                table: NccUserRole,
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccWebSiteInfo}_NccWebSiteId",
                table: NccWebSiteInfo,
                column: "NccWebSiteId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccWebSiteWidget}_WebSiteId",
                table: NccWebSiteWidget,
                column: "WebSiteId");

            migrationBuilder.CreateIndex(
                name: $"IX_{NccWidget}_NccPluginsId",
                table: NccWidget,
                column: "NccPluginsId");

            migrationBuilder.CreateIndex(
                name: $"IX_{IdentityRoleClaim}_RoleId",
                table: IdentityRoleClaim,
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: $"IX_{IdentityUserClaim}_NccUserId",
                table: IdentityUserClaim,
                column: "NccUserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{IdentityUserClaim}_UserId",
                table: IdentityUserClaim,
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{IdentityUserLogin}_NccUserId",
                table: IdentityUserLogin,
                column: "NccUserId");

            migrationBuilder.CreateIndex(
                name: $"IX_{IdentityUserLogin}_UserId",
                table: IdentityUserLogin,
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: NccCategoryDetails);

            migrationBuilder.DropTable(
                name: NccComment);

            migrationBuilder.DropTable(
                name: NccMenuItem);

            migrationBuilder.DropTable(
                name: NccModuleDependency);

            migrationBuilder.DropTable(
                name: NccPageDetails);

            migrationBuilder.DropTable(
                name: NccPageDetailsHistory);

            migrationBuilder.DropTable(
                name: NccPermissionDetails);

            migrationBuilder.DropTable(
                name: NccPostCategory);

            migrationBuilder.DropTable(
                name: NccPostDetails);

            migrationBuilder.DropTable(
                name: NccPostTag);

            migrationBuilder.DropTable(
                name: NccScheduleTaskHistory);

            migrationBuilder.DropTable(
                name: NccSettings);

            migrationBuilder.DropTable(
                name: NccStartup);

            migrationBuilder.DropTable(
                name: NccUserPermission);

            migrationBuilder.DropTable(
                name: NccUserRole);

            migrationBuilder.DropTable(
                name: NccWebSiteInfo);

            migrationBuilder.DropTable(
                name: NccWebSiteWidget);

            migrationBuilder.DropTable(
                name: NccWidget);

            migrationBuilder.DropTable(
                name: NccWidgetSection);

            migrationBuilder.DropTable(
                name: IdentityRoleClaim);

            migrationBuilder.DropTable(
                name: IdentityUserClaim);

            migrationBuilder.DropTable(
                name: IdentityUserLogin);

            migrationBuilder.DropTable(
                name: IdentityUserToken);

            migrationBuilder.DropTable(
                name: NccMenu);

            migrationBuilder.DropTable(
                name: NccModule);

            migrationBuilder.DropTable(
                name: NccPageHistory);

            migrationBuilder.DropTable(
                name: NccCategory);

            migrationBuilder.DropTable(
                name: NccPost);

            migrationBuilder.DropTable(
                name: NccTag);

            migrationBuilder.DropTable(
                name: NccPermission);

            migrationBuilder.DropTable(
                name: NccWebSite);

            migrationBuilder.DropTable(
                name: NccPlugins);

            migrationBuilder.DropTable(
                name: NccRole);

            migrationBuilder.DropTable(
                name: NccPage);

            migrationBuilder.DropTable(
                name: NccUser);
        }
    }
}
