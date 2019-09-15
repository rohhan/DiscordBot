using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class UpdatedPropertyNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Guilds");

            migrationBuilder.AddColumn<decimal>(
                name: "UserDiscordId",
                table: "Users",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildDiscordId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserDiscordId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GuildDiscordId",
                table: "Guilds");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscordId",
                table: "Users",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "Guilds",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
