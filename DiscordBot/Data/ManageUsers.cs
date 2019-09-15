using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Data
{
    public class ManageUsers : IManageUsers
    {
        private DiscordBotDbContext _context;

        public ManageUsers(DiscordBotDbContext context)
        {
            _context = context;
        }

        public async Task UpdateUserActivity(SocketCommandContext socketCommandContext, SocketUserMessage socketUserMessage)
        {
            var user = socketCommandContext.User;

            var guild = socketCommandContext.Guild;

            var userInGuild = _context.GuildUsers.FirstOrDefault(gu => gu.Guild.GuildDiscordId == guild.Id && gu.User.UserDiscordId == user.Id);

            if (userInGuild == null) { return; }

            userInGuild.DateLastActive = DateTimeOffset.Now;

            await _context.SaveChangesAsync();
        }
    }
}
