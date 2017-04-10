using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class initial : Migration
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
                        .Annotation("Sqlite:Autoincrement", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    MenuFor = table.Column<int>(nullable: false),
                    MenuIconCls = table.Column<string>(nullable: true),
                    MenuType = table.Column<int>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    AntiForgery = table.Column<bool>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Dependencies = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleStatus = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true),
                    SortName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    Website = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Module", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Role",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NetCoreCMSVersion = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ThemeName = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false),
                    Website = table.Column<string>(nullable: true)
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
                        .Annotation("Sqlite:Autoincrement", true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    AllowRegistration = table.Column<bool>(nullable: false),
                    Copyrights = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DateFormat = table.Column<string>(nullable: true),
                    DomainName = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    FaviconUrl = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    NewUserRole = table.Column<string>(nullable: true),
                    SiteLogoUrl = table.Column<string>(nullable: true),
                    SiteTitle = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tagline = table.Column<string>(nullable: true),
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
                        .Annotation("Sqlite:Autoincrement", true),
                    Action = table.Column<string>(nullable: true),
                    ActionType = table.Column<int>(nullable: false),
                    Controller = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    MenuIconCls = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Module = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    NccMenuId = table.Column<long>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Target = table.Column<string>(nullable: true),
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
                        name: "FK_Ncc_MenuItem_Ncc_MenuItem_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ncc_MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                name: "Ncc_UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
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
                name: "IX_Ncc_MenuItem_ParentId",
                table: "Ncc_MenuItem",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Ncc_Role",
                column: "NormalizedName",
                unique: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Ncc_Settings");

            migrationBuilder.DropTable(
                name: "Ncc_Theme");

            migrationBuilder.DropTable(
                name: "Ncc_UserRole");

            migrationBuilder.DropTable(
                name: "Ncc_WebSite");

            migrationBuilder.DropTable(
                name: "Ncc_NccMenu");

            migrationBuilder.DropTable(
                name: "Ncc_Role");

            migrationBuilder.DropTable(
                name: "Ncc_User");
        }
    }
}
