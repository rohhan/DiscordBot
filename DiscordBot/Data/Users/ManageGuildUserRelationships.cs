using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data.Users
{
    public class ManageGuildUserRelationships : IManageGuildUserRelationships
    {
        private DiscordBotDbContext _context;

        public ManageGuildUserRelationships(DiscordBotDbContext context)
        {
            _context = context;
        }

        public async Task<GuildUser> CreateGuildUserRelationship(
            SocketGuildUser socketGuildUser, 
            GuildUserActionEnum actionType, 
            DateTimeOffset? actionTime)
        {
            var guild = await _context.Guilds
                .FirstOrDefaultAsync(g => g.GuildDiscordId == socketGuildUser.Guild.Id);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserDiscordId == socketGuildUser.Id);

            if (guild == null)
            {
                throw new Exception();
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = user,
                ActionType = actionType,
                ActionDate = actionTime
            };

            return relationship;
        }

        public async Task<GuildUser> CreateGuildUserRelationship(
            SocketUser socketUser, 
            SocketGuild socketGuild, 
            GuildUserActionEnum actionType, 
            DateTimeOffset? actionTime)
        {
            var guild = await _context.Guilds
                .FirstOrDefaultAsync(g => g.GuildDiscordId == socketGuild.Id);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserDiscordId == socketUser.Id);

            if (guild == null)
            {
                // Todo: Create guild
                throw new Exception();
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = user,
                ActionType = actionType,
                ActionDate = actionTime
            };

            return relationship;
        }
    }
}
