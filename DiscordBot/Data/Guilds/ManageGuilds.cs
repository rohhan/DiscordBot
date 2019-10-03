using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBot.Data.Users;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Data.Guilds
{
    public class ManageGuilds : IManageGuilds
    {
        private DiscordBotDbContext _context;
        private IAddUsers _addUsersRepo;

        public ManageGuilds(DiscordBotDbContext context, IAddUsers addUsersRepo)
        {
            _context = context;
            _context.Database.EnsureCreated();
            _addUsersRepo = addUsersRepo;
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
                bool userAdded = await _addUsersRepo.AddNewUser(socketGuildUser);

                if (userAdded)
                {
                    newUsers++;
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
    }
}
