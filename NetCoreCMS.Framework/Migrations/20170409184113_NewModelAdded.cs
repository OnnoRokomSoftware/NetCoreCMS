using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class NewModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "BaseModel",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Ncc_User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Ncc_Role",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "BaseModel");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Ncc_User");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Ncc_Role");
        }
    }
}
