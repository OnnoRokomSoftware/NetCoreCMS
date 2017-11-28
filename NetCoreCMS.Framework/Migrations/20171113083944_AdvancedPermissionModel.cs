using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class AdvancedPermissionModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ncc_Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: false),
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
                name: "Ncc_Schedule_Task_History",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Data = table.Column<string>(type: "longtext", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TaskCreator = table.Column<string>(type: "longtext", nullable: true),
                    TaskId = table.Column<string>(type: "longtext", nullable: true),
                    TaskOf = table.Column<string>(type: "longtext", nullable: true),
                    TaskType = table.Column<string>(type: "longtext", nullable: true),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Schedule_Task_History", x => x.Id);
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
                    ExtraAllowUserId = table.Column<long>(type: "bigint", nullable: true),
                    ExtraDenyUserId = table.Column<long>(type: "bigint", nullable: true),
                    Metadata = table.Column<string>(type: "longtext", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifyBy = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<string>(type: "longtext", nullable: true),
                    MenuType = table.Column<string>(type: "longtext", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: true),
                    Requirements = table.Column<string>(type: "longtext", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Permission_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Permission_Details_Ncc_User_ExtraAllowUserId",
                        column: x => x.ExtraAllowUserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Permission_Details_Ncc_User_ExtraDenyUserId",
                        column: x => x.ExtraDenyUserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ncc_Permission_Details_Ncc_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Ncc_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ncc_User_Permission",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_User_Permission", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_Ncc_User_Permission_Ncc_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Ncc_Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ncc_User_Permission_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Permission_Details_ExtraAllowUserId",
                table: "Ncc_Permission_Details",
                column: "ExtraAllowUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Permission_Details_ExtraDenyUserId",
                table: "Ncc_Permission_Details",
                column: "ExtraDenyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Permission_Details_PermissionId",
                table: "Ncc_Permission_Details",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_User_Permission_PermissionId",
                table: "Ncc_User_Permission",
                column: "PermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ncc_Permission_Details");

            migrationBuilder.DropTable(
                name: "Ncc_Schedule_Task_History");

            migrationBuilder.DropTable(
                name: "Ncc_User_Permission");

            migrationBuilder.DropTable(
                name: "Ncc_Permission");
        }
    }
}
