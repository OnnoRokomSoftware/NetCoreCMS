using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class nccStartup_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ncc_Startup",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGeneratedOnAdd", true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RoleId = table.Column<long>(nullable: true),
                    StartupFor = table.Column<int>(nullable: false),
                    StartupType = table.Column<int>(nullable: false),
                    StartupUrl = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ncc_Startup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ncc_Startup_Ncc_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ncc_Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Ncc_Startup_Ncc_User_UserId",
                        column: x => x.UserId,
                        principalTable: "Ncc_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Startup_RoleId",
                table: "Ncc_Startup",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Startup_UserId",
                table: "Ncc_Startup",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ncc_Startup");
        }
    }
}
