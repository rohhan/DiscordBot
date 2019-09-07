using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class AddedNewPropertiesToTrackDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateLastActive",
                table: "GuildUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateLeft",
                table: "GuildUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateAdded",
                table: "Guilds",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateLastActive",
                table: "GuildUsers");

            migrationBuilder.DropColumn(
                name: "DateLeft",
                table: "GuildUsers");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Guilds");
        }
    }
}
