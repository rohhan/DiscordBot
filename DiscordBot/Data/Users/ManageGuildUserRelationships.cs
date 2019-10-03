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

        public async Task<GuildUser> CreateGuildUserRelationship(SocketGuildUser socketGuildUser, GuildUserActionEnum actionType, DateTimeOffset? actionTime)
        {
            var guild = await _context.Guilds.FirstOrDefaultAsync(g => g.GuildDiscordId == socketGuildUser.Guild.Id);

            if (guild == null)
            {
                throw new Exception();
            }

            var relationship = new GuildUser()
            {
                Guild = guild,
                User = Map(socketGuildUser),
                ActionType = actionType,
                ActionDate = actionTime
            };

            return relationship;
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
