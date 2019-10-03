using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class GuildUserActionEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "GuildUsers");

            migrationBuilder.RenameColumn(
                name: "DateLeft",
                table: "GuildUsers",
                newName: "ActionDate");

            migrationBuilder.AddColumn<int>(
                name: "ActionType",
                table: "GuildUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "GuildUsers");

            migrationBuilder.RenameColumn(
                name: "ActionDate",
                table: "GuildUsers",
                newName: "DateLeft");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateJoined",
                table: "GuildUsers",
                nullable: true);
        }
    }
}
