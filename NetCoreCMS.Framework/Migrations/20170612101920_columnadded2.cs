using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class columnadded2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Ncc_Module",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleTitle",
                table: "Ncc_Module",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WebSite",
                table: "Ncc_Module",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Ncc_Module");

            migrationBuilder.DropColumn(
                name: "ModuleTitle",
                table: "Ncc_Module");

            migrationBuilder.DropColumn(
                name: "WebSite",
                table: "Ncc_Module");
        }
    }
}
