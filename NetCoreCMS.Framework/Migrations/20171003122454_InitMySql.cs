/*************************************************************
 *          Project: NetCoreCMS                              *
 *              Web: http://dotnetcorecms.org                *
 *           Author: OnnoRokom Software Ltd.                 *
 *          Website: www.onnorokomsoftware.com               *
 *            Email: info@onnorokomsoftware.com              *
 *        Copyright: OnnoRokom Software Ltd.                 *
 *          License: BSD-3-Clause                            *
 *************************************************************/

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class InitMySql : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ncc_Category",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryImage = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Category_Ncc_Category_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Menu",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MenuIconCls = table.Column<string>(type: "longtext", nullable: true),
                    MenuLanguage = table.Column<string>(type: "longtext", nullable: true),
                    MenuOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Position = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Module",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AntiForgery = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<string>(type: "longtext", nullable: true),
                    Category = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Folder = table.Column<string>(type: "longtext", nullable: true),
                    MaxNccVersion = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    MinNccVersion = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<string>(type: "longtext", nullable: true),
                    IsCore = table.Column<bool>(type: "bit", nullable: false, defaultValue:false),
                    ModuleStatus = table.Column<int>(type: "int", nullable: false),
                    ModuleTitle = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Path = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    WebSite = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Page",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Layout = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    PageOrder = table.Column<int>(type: "int", nullable: false),
                    PageStatus = table.Column<int>(type: "int", nullable: false),
                    PageType = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AntiForgery = table.Column<bool>(type: "bit", nullable: false),
                    Author = table.Column<string>(type: "longtext", nullable: true),
                    Category = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Dependencies = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NetCoreCMSVersion = table.Column<string>(type: "longtext", nullable: true),
                    Path = table.Column<string>(type: "longtext", nullable: true),
                    PluginsStatus = table.Column<int>(type: "int", nullable: false),
                    SortName = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Version = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    Website = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Plugins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Slug = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Settings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    GroupId = table.Column<string>(type: "longtext", nullable: true),
                    Key = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Tag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FullName = table.Column<string>(type: "longtext", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    Mobile = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    Slug = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_WebSite",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllowRegistration = table.Column<bool>(type: "bit", nullable: false),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateFormat = table.Column<string>(type: "longtext", nullable: true),
                    DomainName = table.Column<string>(type: "longtext", nullable: true),
                    EmailAddress = table.Column<string>(type: "longtext", nullable: true),
                    GoogleAnalyticsId = table.Column<string>(type: "longtext", nullable: true),
                    IsMultiLangual = table.Column<bool>(type: "bit", nullable: false),
                    IsShowFullPost = table.Column<bool>(type: "bit", nullable: false),
                    Language = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NewUserRole = table.Column<string>(type: "longtext", nullable: true),
                    PerPagePostSize = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TimeFormat = table.Column<string>(type: "longtext", nullable: true),
                    TimeZone = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    TablePrefix = table.Column<string>(type: "longtext", nullable: true),
                    EnableCache = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_WebSite", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Widget_Sections",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Dependencies = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NetCoreCMSVersion = table.Column<string>(type: "longtext", nullable: true),
                    SortName = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Widget_Sections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Category_Details",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Language = table.Column<string>(type: "longtext", nullable: true),
                    MetaDescription = table.Column<string>(type: "longtext", nullable: true),
                    MetaKeyword = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Slug = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Category_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Category_Details_Ncc_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Ncc_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Menu_Item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(type: "longtext", nullable: true),
                    Controller = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: true),
                    MenuActionType = table.Column<int>(type: "int", nullable: false),
                    MenuFor = table.Column<int>(type: "int", nullable: false),
                    MenuIconCls = table.Column<string>(type: "longtext", nullable: true),
                    MenuOrder = table.Column<int>(type: "int", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Module = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NccMenuId = table.Column<long>(type: "bigint", nullable: true),
                    NccMenuItemId = table.Column<long>(type: "bigint", nullable: true),
                    NccMenuItemId1 = table.Column<long>(type: "bigint", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Target = table.Column<string>(type: "longtext", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Menu_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Menu_Item_Ncc_Menu_NccMenuId",
                        column: x => x.NccMenuId,
                        principalTable: "Ncc_Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Menu_Item_Ncc_Menu_Item_NccMenuItemId",
                        column: x => x.NccMenuItemId,
                        principalTable: "Ncc_Menu_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Menu_Item_Ncc_Menu_Item_NccMenuItemId1",
                        column: x => x.NccMenuItemId1,
                        principalTable: "Ncc_Menu_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Menu_Item_Ncc_Menu_Item_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_Menu_Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Module_Dependency",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MaxVersion = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    MinVersion = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NccModuleId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Module_Dependency", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Module_Dependency_Ncc_Module_NccModuleId",
                        column: x => x.NccModuleId,
                        principalTable: "Ncc_Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Page_Details",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Language = table.Column<string>(type: "longtext", nullable: true),
                    MetaDescription = table.Column<string>(type: "longtext", nullable: true),
                    MetaKeyword = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    PageId = table.Column<long>(type: "bigint", nullable: true),
                    Slug = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Page_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Page_Details_Ncc_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Ncc_Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Widget",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<byte[]>(type: "longblob", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Dependencies = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NccPluginsId = table.Column<long>(type: "bigint", nullable: true),
                    NetCoreCMSVersion = table.Column<string>(type: "longtext", nullable: true),
                    SortName = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
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
                name: "Ncc_Role_Claim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Role_Claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Role_Claim_Ncc_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ncc_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Startup",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    StartupFor = table.Column<int>(type: "int", nullable: false),
                    StartupType = table.Column<int>(type: "int", nullable: false),
                    StartupUrl = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Startup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Startup_Ncc_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ncc_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Post",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AllowComment = table.Column<bool>(type: "bit", nullable: false),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsStiky = table.Column<bool>(type: "bit", nullable: false),
                    Layout = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    PostStatus = table.Column<int>(type: "int", nullable: false),
                    PostType = table.Column<int>(type: "int", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CommentCount = table.Column<long>(type: "bigint", nullable: false, defaultValue:0),
                    RelatedPosts = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ThumImage = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Ncc_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Ncc_Post_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User_Claim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true),
                    NccUserId = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Claim_Ncc_User_NccUserId",
                        column: x => x.NccUserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Claim_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User_Login",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(127)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(127)", nullable: false),
                    NccUserId = table.Column<long>(type: "bigint", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Login", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Ncc_User_Login_Ncc_User_NccUserId",
                        column: x => x.NccUserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Login_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User_Role",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Role", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Ncc_User_Role_Ncc_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ncc_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Role_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User_Token",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(127)", nullable: false),
                    Name = table.Column<string>(type: "varchar(127)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Token", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_Ncc_User_Token_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_WebSite_Info",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Copyrights = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FaviconUrl = table.Column<string>(type: "longtext", nullable: true),
                    Language = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    NccWebSiteId = table.Column<long>(type: "bigint", nullable: true),
                    PrivacyPolicyUrl = table.Column<string>(type: "longtext", nullable: true),
                    SiteLogoUrl = table.Column<string>(type: "longtext", nullable: true),
                    SiteTitle = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tagline = table.Column<string>(type: "longtext", nullable: true),
                    TermsAndConditionsUrl = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_WebSite_Info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_WebSite_Info_Ncc_WebSite_NccWebSiteId",
                        column: x => x.NccWebSiteId,
                        principalTable: "Ncc_WebSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_WebSite_Widget",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LayoutName = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    WebSiteId = table.Column<long>(type: "bigint", nullable: true),
                    WidgetConfigJson = table.Column<string>(type: "longtext", nullable: true),
                    WidgetData = table.Column<string>(type: "longtext", nullable: true),
                    WidgetId = table.Column<string>(type: "longtext", nullable: true),
                    WidgetOrder = table.Column<int>(type: "int", nullable: false),
                    Zone = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_WebSite_Widget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_WebSite_Widget_Ncc_WebSite_WebSiteId",
                        column: x => x.WebSiteId,
                        principalTable: "Ncc_WebSite",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Post_Category",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Post_Category", x => new { x.PostId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Category_Ncc_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Ncc_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Category_Ncc_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Ncc_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Post_Comment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AuthorId = table.Column<long>(type: "bigint", nullable: true),
                    Content = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    PostId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    AuthorName = table.Column<string>(type: "longtext", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: true),
                    WebSite = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false),
                    CommentStatus = table.Column<int>(type: "int", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Post_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Comment_Ncc_User_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Comment_Ncc_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Ncc_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Post_Details",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Language = table.Column<string>(type: "longtext", nullable: true),
                    MetaDescription = table.Column<string>(type: "longtext", nullable: true),
                    MetaKeyword = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    PostId = table.Column<long>(type: "bigint", nullable: true),
                    Slug = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Post_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Details_Ncc_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Ncc_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Post_Tag",
                columns: table => new
                {
                    PostId = table.Column<long>(type: "bigint", nullable: false),
                    TagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Post_Tag", x => new { x.PostId, x.TagId });
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Tag_Ncc_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Ncc_Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_Post_Tag_Ncc_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Ncc_Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Category_ParentId",
                table: "Ncc_Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Category_Details_CategoryId",
                table: "Ncc_Category_Details",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Menu_Item_NccMenuId",
                table: "Ncc_Menu_Item",
                column: "NccMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Menu_Item_NccMenuItemId",
                table: "Ncc_Menu_Item",
                column: "NccMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Menu_Item_NccMenuItemId1",
                table: "Ncc_Menu_Item",
                column: "NccMenuItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Menu_Item_ParentId",
                table: "Ncc_Menu_Item",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Module_Dependency_NccModuleId",
                table: "Ncc_Module_Dependency",
                column: "NccModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Page_ParentId",
                table: "Ncc_Page",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Page_Details_PageId",
                table: "Ncc_Page_Details",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_AuthorId",
                table: "Ncc_Post",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_ParentId",
                table: "Ncc_Post",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_Category_CategoryId",
                table: "Ncc_Post_Category",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_Comment_AuthorId",
                table: "Ncc_Post_Comment",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_Comment_PostId",
                table: "Ncc_Post_Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_Details_PostId",
                table: "Ncc_Post_Details",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Post_Tag_TagId",
                table: "Ncc_Post_Tag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Ncc_Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Role_Claim_RoleId",
                table: "Ncc_Role_Claim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Startup_RoleId",
                table: "Ncc_Startup",
                column: "RoleId");

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
                name: "IX_Ncc_User_Claim_NccUserId",
                table: "Ncc_User_Claim",
                column: "NccUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Claim_UserId",
                table: "Ncc_User_Claim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Login_NccUserId",
                table: "Ncc_User_Login",
                column: "NccUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Login_UserId",
                table: "Ncc_User_Login",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Role_RoleId",
                table: "Ncc_User_Role",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_WebSite_Info_NccWebSiteId",
                table: "Ncc_WebSite_Info",
                column: "NccWebSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_WebSite_Widget_WebSiteId",
                table: "Ncc_WebSite_Widget",
                column: "WebSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Widget_NccPluginsId",
                table: "Ncc_Widget",
                column: "NccPluginsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ncc_Category_Details");

            migrationBuilder.DropTable(
                name: "Ncc_Menu_Item");

            migrationBuilder.DropTable(
                name: "Ncc_Module_Dependency");

            migrationBuilder.DropTable(
                name: "Ncc_Page_Details");

            migrationBuilder.DropTable(
                name: "Ncc_Post_Category");

            migrationBuilder.DropTable(
                name: "Ncc_Post_Comment");

            migrationBuilder.DropTable(
                name: "Ncc_Post_Details");

            migrationBuilder.DropTable(
                name: "Ncc_Post_Tag");

            migrationBuilder.DropTable(
                name: "Ncc_Role_Claim");

            migrationBuilder.DropTable(
                name: "Ncc_Settings");

            migrationBuilder.DropTable(
                name: "Ncc_Startup");

            migrationBuilder.DropTable(
                name: "Ncc_User_Claim");

            migrationBuilder.DropTable(
                name: "Ncc_User_Login");

            migrationBuilder.DropTable(
                name: "Ncc_User_Role");

            migrationBuilder.DropTable(
                name: "Ncc_User_Token");

            migrationBuilder.DropTable(
                name: "Ncc_WebSite_Info");

            migrationBuilder.DropTable(
                name: "Ncc_WebSite_Widget");

            migrationBuilder.DropTable(
                name: "Ncc_Widget");

            migrationBuilder.DropTable(
                name: "Ncc_Widget_Sections");

            migrationBuilder.DropTable(
                name: "Ncc_Menu");

            migrationBuilder.DropTable(
                name: "Ncc_Module");

            migrationBuilder.DropTable(
                name: "Ncc_Page");

            migrationBuilder.DropTable(
                name: "Ncc_Category");

            migrationBuilder.DropTable(
                name: "Ncc_Post");

            migrationBuilder.DropTable(
                name: "Ncc_Tag");

            migrationBuilder.DropTable(
                name: "Ncc_Role");

            migrationBuilder.DropTable(
                name: "Ncc_WebSite");

            migrationBuilder.DropTable(
                name: "Ncc_Plugins");

            migrationBuilder.DropTable(
                name: "Ncc_User");
        }
    }
}
