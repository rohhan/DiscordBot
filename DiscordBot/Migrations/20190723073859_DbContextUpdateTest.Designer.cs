﻿// <auto-generated />
using DiscordBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DiscordBot.Migrations
{
    [DbContext(typeof(DiscordBotDbContext))]
    [Migration("20190723073859_DbContextUpdateTest")]
    partial class DbContextUpdateTest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DiscordBot.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("GuildId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<decimal>("OwnerId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("DiscordBot.Models.GuildUser", b =>
                {
                    b.Property<int>("GuildId");

                    b.Property<int>("UserId");

                    b.HasKey("GuildId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GuildUser");
                });

            modelBuilder.Entity("DiscordBot.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("DiscordId");

                    b.Property<int>("DiscriminatorValue");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DiscordBot.Models.GuildUser", b =>
                {
                    b.HasOne("DiscordBot.Models.Guild", "Guild")
                        .WithMany("GuildUsers")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DiscordBot.Models.User", "User")
                        .WithMany("UserGuilds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
