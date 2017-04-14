using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class columnadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostType",
                table: "Ncc_NccPost",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Ncc_NccPost",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "AddToNavigationMenu",
                table: "Ncc_Page",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PageStatus",
                table: "Ncc_Page",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PageType",
                table: "Ncc_Page",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Ncc_Page",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Ncc_NccPost");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Ncc_NccPost");

            migrationBuilder.DropColumn(
                name: "AddToNavigationMenu",
                table: "Ncc_Page");

            migrationBuilder.DropColumn(
                name: "PageStatus",
                table: "Ncc_Page");

            migrationBuilder.DropColumn(
                name: "PageType",
                table: "Ncc_Page");

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Ncc_Page");
        }
    }
}
