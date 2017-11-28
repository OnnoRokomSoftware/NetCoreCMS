using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class Role_Startup_Changed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ncc_Startup_Ncc_Role_RoleId",
                table: "Ncc_Startup");

            migrationBuilder.DropIndex(
                name: "IX_Ncc_Startup_RoleId",
                table: "Ncc_Startup");

            migrationBuilder.DropColumn(
                name: "PerPagePostSize",
                table: "Ncc_WebSite");

            migrationBuilder.AddColumn<int>(
                name: "AdminPageSize",
                table: "Ncc_WebSite",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WebSitePageSize",
                table: "Ncc_WebSite",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PermissionId",
                table: "Ncc_Startup",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Startup_PermissionId",
                table: "Ncc_Startup",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ncc_Startup_Ncc_Permission_PermissionId",
                table: "Ncc_Startup",
                column: "PermissionId",
                principalTable: "Ncc_Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ncc_Startup_Ncc_Permission_PermissionId",
                table: "Ncc_Startup");

            migrationBuilder.DropIndex(
                name: "IX_Ncc_Startup_PermissionId",
                table: "Ncc_Startup");

            migrationBuilder.DropColumn(
                name: "AdminPageSize",
                table: "Ncc_WebSite");

            migrationBuilder.DropColumn(
                name: "WebSitePageSize",
                table: "Ncc_WebSite");

            migrationBuilder.DropColumn(
                name: "PermissionId",
                table: "Ncc_Startup");

            migrationBuilder.AddColumn<int>(
                name: "PerPagePostSize",
                table: "Ncc_WebSite",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_Startup_RoleId",
                table: "Ncc_Startup",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ncc_Startup_Ncc_Role_RoleId",
                table: "Ncc_Startup",
                column: "RoleId",
                principalTable: "Ncc_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
