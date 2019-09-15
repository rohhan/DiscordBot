using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Data
{
    public class InitializeGuilds : IInitializeGuilds
    {
        private DiscordBotDbContext _context;

        public InitializeGuilds(DiscordBotDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<bool> AddNewGuild(SocketGuild socketGuild)
        {
            var alreadyExists = _context.Guilds.Any(g => g.GuildDiscordId == socketGuild.Id);

            if (alreadyExists)
            {
                return false;
            }

            var guild = Map(socketGuild);

            await _context.Guilds.AddAsync(guild);

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<int> UpdateGuildUsers(SocketGuild socketGuild)
        {
            var socketGuildUsers = socketGuild.Users;

            var newUsers = 0;

            foreach (var socketGuildUser in socketGuildUsers)
            {
                // Save User
                var userAlreadyExists = _context.Users.Any(u => u.UserDiscordId == socketGuildUser.Id);

                var user = Map(socketGuildUser);

                if (!userAlreadyExists)
                {
                    newUsers++;

                    await _context.Users.AddAsync(user);

                    await _context.SaveChangesAsync();
                }

                // Save Guild User Relationship
                var relationshipAlreadyExists = _context.GuildUsers.Any(gu => gu.Guild.GuildDiscordId == socketGuild.Id && gu.User.UserDiscordId == socketGuildUser.Id);

                var guild = await _context.Guilds.FirstOrDefaultAsync(g => g.GuildDiscordId == socketGuild.Id);

                if (!relationshipAlreadyExists)
                {
                    var relationship = new GuildUser()
                    {
                        Guild = guild,
                        User = user,
                        DateJoined = socketGuildUser.JoinedAt
                    };

                    await _context.GuildUsers.AddAsync(relationship);

                    await _context.SaveChangesAsync();
                }
            }

            return newUsers;
        }

        private Guild Map(SocketGuild guildParam)
        {
            var guild = new Guild
            {
                DateAdded = DateTimeOffset.Now,
                DateCreated = guildParam.CreatedAt,
                GuildDiscordId = guildParam.Id,
                GuildName = guildParam.Name,
                OwnerId = guildParam.OwnerId,
                GuildUsers = new List<GuildUser>()
            };

            return guild;
        }

        private User Map(SocketGuildUser socketGuildUser)
        {
            return new User()
            {
                UserDiscordId = socketGuildUser.Id,
                DiscriminatorValue = socketGuildUser.DiscriminatorValue,
                Username = socketGuildUser.Username,
                IsBot = socketGuildUser.IsBot
            };
        }
    }
}
