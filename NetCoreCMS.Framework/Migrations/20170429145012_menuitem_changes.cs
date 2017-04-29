using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class menuitem_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "Title",
            //    table: "Ncc_Page",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "Slug",
            //    table: "Ncc_Page",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NccMenuItemId1",
                table: "Ncc_MenuItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ncc_MenuItem_NccMenuItemId1",
                table: "Ncc_MenuItem",
                column: "NccMenuItemId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Ncc_MenuItem_Ncc_MenuItem_NccMenuItemId1",
            //    table: "Ncc_MenuItem",
            //    column: "NccMenuItemId1",
            //    principalTable: "Ncc_MenuItem",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ncc_MenuItem_Ncc_MenuItem_NccMenuItemId1",
                table: "Ncc_MenuItem");

            migrationBuilder.DropIndex(
                name: "IX_Ncc_MenuItem_NccMenuItemId1",
                table: "Ncc_MenuItem");

            migrationBuilder.DropColumn(
                name: "NccMenuItemId1",
                table: "Ncc_MenuItem");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Ncc_Page",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Ncc_Page",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
