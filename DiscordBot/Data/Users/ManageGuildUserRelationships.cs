using Discord.WebSocket;
using DiscordBot.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> CreateGuildUserRelationship(
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
                // Todo: Create guild
                throw new Exception();
            }

            if (user == null)
            {
                // Todo: Create user
                throw new Exception();
            }

            var mostRecentGuildUserRelationship = _context.GuildUsers
                .OrderByDescending(gu => gu.ActionDate)
                .FirstOrDefault(gu => gu.Guild.GuildDiscordId == socketGuildUser.Guild.Id && gu.User.UserDiscordId == socketGuildUser.Id);

            if (mostRecentGuildUserRelationship != null &&
                mostRecentGuildUserRelationship.ActionType == actionType &&
                mostRecentGuildUserRelationship.ActionDate == actionTime)
            {
                return false;
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = user,
                ActionType = actionType,
                ActionDate = actionTime
            };

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CreateGuildUserRelationship(
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

            if (user == null)
            {
                // Todo: Create user
                throw new Exception();
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = user,
                ActionType = actionType,
                ActionDate = actionTime
            };

            await _context.GuildUsers.AddAsync(relationship);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
