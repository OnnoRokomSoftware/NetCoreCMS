using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class advanced_authorization_models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ncc_User_Authorization");

            migrationBuilder.CreateTable(
                name: "Ncc_Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Group = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_Permission_Details",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(type: "longtext", nullable: true),
                    Controller = table.Column<string>(type: "longtext", nullable: true),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExtraDeniedUserId = table.Column<long>(type: "bigint", nullable: false),
                    ExtraPermissionUserId = table.Column<long>(type: "bigint", nullable: false),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
                    Requirements = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Permission_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Permission_Details_Ncc_User_ExtraDeniedUserId",
                        column: x => x.ExtraDeniedUserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_Permission_Details_Ncc_User_ExtraPermissionUserId",
                        column: x => x.ExtraPermissionUserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_Permission_Details_Ncc_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Ncc_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User_Permission",
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
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Permission_Ncc_User_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Permission_Ncc_Permission_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Permission_Details_ExtraDeniedUserId",
                table: "Ncc_Permission_Details",
                column: "ExtraDeniedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Permission_Details_ExtraPermissionUserId",
                table: "Ncc_Permission_Details",
                column: "ExtraPermissionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Permission_Details_PermissionId",
                table: "Ncc_Permission_Details",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Permission_PermissionId",
                table: "Ncc_User_Permission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Permission_UserId",
                table: "Ncc_User_Permission",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ncc_Permission_Details");

            migrationBuilder.DropTable(
                name: "Ncc_User_Permission");

            migrationBuilder.DropTable(
                name: "Ncc_Permission");

            migrationBuilder.CreateTable(
                name: "Ncc_User_Authorization",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Action = table.Column<string>(nullable: true),
                    Controller = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    ModuleId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RequirementName = table.Column<string>(nullable: true),
                    RequirementValue = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Authorization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Authorization_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Authorization_UserId",
                table: "Ncc_User_Authorization",
                column: "UserId");
        }
    }
}
