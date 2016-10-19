using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FlexLabs.EDAssistant.Repositories.EFCore.Base.Migrations
{
    public partial class ServerWelcomes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WelcomeMessage",
                table: "Servers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommandPrefix",
                table: "Servers",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WelcomeMessage",
                table: "Servers");

            migrationBuilder.AlterColumn<string>(
                name: "CommandPrefix",
                table: "Servers",
                maxLength: 5,
                nullable: false);
        }
    }
}
