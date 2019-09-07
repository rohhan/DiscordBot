using System;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Data
{
    public class DiscordBotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildUser> GuildUsers { get; set; }

        public DiscordBotDbContext(DbContextOptions<DiscordBotDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuildUser>()
                .HasKey(g => new { g.GuildId, g.UserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
