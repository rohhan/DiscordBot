using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordBot.Migrations
{
    public partial class UpdatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildUser_Guilds_GuildId",
                table: "GuildUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildUser_Users_UserId",
                table: "GuildUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildUser",
                table: "GuildUser");

            migrationBuilder.RenameTable(
                name: "GuildUser",
                newName: "GuildUsers");

            migrationBuilder.RenameIndex(
                name: "IX_GuildUser_UserId",
                table: "GuildUsers",
                newName: "IX_GuildUsers_UserId");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscordId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateJoined",
                table: "GuildUsers",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildUsers",
                table: "GuildUsers",
                columns: new[] { "GuildId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GuildUsers_Guilds_GuildId",
                table: "GuildUsers",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildUsers_Users_UserId",
                table: "GuildUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuildUsers_Guilds_GuildId",
                table: "GuildUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GuildUsers_Users_UserId",
                table: "GuildUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuildUsers",
                table: "GuildUsers");

            migrationBuilder.RenameTable(
                name: "GuildUsers",
                newName: "GuildUser");

            migrationBuilder.RenameIndex(
                name: "IX_GuildUsers_UserId",
                table: "GuildUser",
                newName: "IX_GuildUser_UserId");

            migrationBuilder.AlterColumn<long>(
                name: "DiscordId",
                table: "Users",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateJoined",
                table: "GuildUser",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuildUser",
                table: "GuildUser",
                columns: new[] { "GuildId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GuildUser_Guilds_GuildId",
                table: "GuildUser",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GuildUser_Users_UserId",
                table: "GuildUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
