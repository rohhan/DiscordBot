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

        public ManageUserActivity(DiscordBotDbContext context)
        {
            _context = context;
        }

        public async Task UpdateUserActivity(SocketCommandContext socketCommandContext, SocketUserMessage socketUserMessage)
        {
            var user = socketCommandContext.User;

            var guild = socketCommandContext.Guild;

            var userInGuild = _context.GuildUsers
                .OrderByDescending(gu => gu.ActionDate)
                .FirstOrDefault(gu => gu.Guild.GuildDiscordId == guild.Id && gu.User.UserDiscordId == user.Id);

            if (userInGuild == null) 
            { 
                // Todo: Add user to guild
            }

            userInGuild.DateLastActive = DateTimeOffset.Now;

            await _context.SaveChangesAsync();
        }
    }
}
