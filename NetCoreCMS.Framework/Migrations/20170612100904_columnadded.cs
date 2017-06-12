using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class columnadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Ncc_Module",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Ncc_Module",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Ncc_Module");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Ncc_Module");
        }
    }
}
