using Discord.Commands;
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
    public class ManageUserActivity : IManageUserActivity
    {
        private DiscordBotDbContext _context;
        private IManageGuildUserRelationships _relationshipRepo;

        public ManageUserActivity(DiscordBotDbContext context,
            IManageGuildUserRelationships relationshipRepo)
        {
            _context = context;
            _relationshipRepo = relationshipRepo;
        }

        public async Task UpdateUserActivity(SocketCommandContext socketCommandContext, SocketUserMessage socketUserMessage)
        {
            var socketUser = socketCommandContext.User;

            var socketGuild = socketCommandContext.Guild;

            var userInGuild = _context.GuildUsers
                .OrderByDescending(gu => gu.ActionDate)
                .FirstOrDefault(gu => gu.Guild.GuildDiscordId == socketGuild.Id && gu.User.UserDiscordId == socketUser.Id);

            if (userInGuild == null) 
            {
                // Todo: Add guild user relationship
                // Todo: If latest guild user relationship isn't a "JOIN", add that too
            }

            userInGuild.DateLastActive = DateTimeOffset.Now;

            await _context.SaveChangesAsync();
        }
    }
}
