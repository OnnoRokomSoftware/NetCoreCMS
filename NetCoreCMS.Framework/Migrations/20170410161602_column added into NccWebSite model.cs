using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NetCoreCMS.Framework.Setup;
using NetCoreCMS.Framework.Core.Data;

namespace NetCoreCMS.Framework.Migrations
{
    public partial class columnaddedintoNccWebSitemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (SetupHelper.SelectedDatabase != Enum.GetName(typeof(DatabaseEngine), DatabaseEngine.SqLite))
            {
                migrationBuilder.AlterColumn<string>(
                name: "SiteTitle",
                table: "Ncc_WebSite",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
            }
            migrationBuilder.AddColumn<string>(
                name: "PrivacyPolicyUrl",
                table: "Ncc_WebSite",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditionsUrl",
                table: "Ncc_WebSite",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (SetupHelper.SelectedDatabase != Enum.GetName(typeof(DatabaseEngine), DatabaseEngine.SqLite))
            {
                migrationBuilder.DropColumn(
                name: "PrivacyPolicyUrl",
                table: "Ncc_WebSite");

                migrationBuilder.DropColumn(
                    name: "TermsAndConditionsUrl",
                    table: "Ncc_WebSite");

                migrationBuilder.AlterColumn<string>(
                    name: "SiteTitle",
                    table: "Ncc_WebSite",
                    nullable: true,
                    oldClrType: typeof(string));
            }            
        }
    }
}
